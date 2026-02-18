using Koishibot.Core.Features.AdBreak.Events;
using Koishibot.Core.Features.ChannelPoints.Events;
using Koishibot.Core.Features.ChatMessages.Events;
using Koishibot.Core.Features.Moderation;
using Koishibot.Core.Features.Polls.Events;
using Koishibot.Core.Features.Predictions.Events;
using Koishibot.Core.Features.Raids.Events;
using Koishibot.Core.Features.Shoutouts;
using Koishibot.Core.Features.StreamInformation;
using Koishibot.Core.Features.StreamInformation.Events;
using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Features.Vips.Events;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatSettings;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ShieldMode;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.StreamStatus;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Vip;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;
using Koishibot.Core.Services.Twitch.Exceptions;
using Koishibot.Core.Services.Twitch.Extensions;
using Koishibot.Core.Services.TwitchApi.Models;
using Koishibot.Core.Services.Websockets;
using System.Text.Json;
using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Shoutouts.Events;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Koishibot.Core.Services.Twitch.EventSubs;

/*═══════════════════【 SERVICE 】═══════════════════*/
public class TwitchEventSubService : ITwitchEventSubService
{
	public TwitchEventSubService(IAppCache cache,
		IOptions<Settings> settings,
		IServiceScopeFactory scopeFactory,
		ISignalrService signalr,
		ILogger<TwitchEventSubService> log,
		ITwitchApiRequest twitchApiRequest)
	{
		Cache = cache;
		Settings = settings;
		ScopeFactory = scopeFactory;
		Signalr = signalr;
		Log = log;
		TwitchApiRequest = twitchApiRequest;

		Timer.Elapsed += TimerElapsed;
		Timer.AutoReset = false;
	}
	public CancellationToken? Cancel { get; set; }

	private WebSocketFactory Factory { get; set; } = new();
	private WebSocketHandler? TwitchEventSub { get; set; }

	private int? _timeoutSeconds = 60;
	private Timer Timer { get; } = new(TimeSpan.FromSeconds(63));
	private bool _useCli = false; // For testing on debug
	private TaskCompletionSource? _reconnectCompleted;
	private readonly object _lock = new object();
	private string sessionId { get; set; } = "";
	public IAppCache Cache { get; init; }
	public IOptions<Settings> Settings { get; init; }
	public IServiceScopeFactory ScopeFactory { get; init; }
	public ISignalrService Signalr { get; init; }
	public ILogger<TwitchEventSubService> Log { get; init; }
	public ITwitchApiRequest TwitchApiRequest { get; init; }

	private readonly object _reconnectLock = new();

	private readonly LimitedSizeHashSet<Metadata, string> _eventSet
		= new(25, x => x.MessageId);

	public async Task CreateWebSocket()
	{
		var url = _useCli
			? $"ws://127.0.0.1:8080/ws?keepalive_timeout_seconds={_timeoutSeconds}"
			: $"wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds={_timeoutSeconds}";

		try
		{
			TwitchEventSub = await Factory.Create(url, 3, ProcessMessage, Error, Closed);
		}
		catch (Exception e)
		{
			await Signalr.SendError(e.Message);
			throw new CustomException(e.Message);
		}
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		try
		{
			if (TwitchEventSub is null) { return; }

			if (message.IsPing())
			{
				await TwitchEventSub.SendMessage("PONG");
				return;
			}

			var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message.Message);
			if (eventMessage == null)
				throw new JsonDeserializeException(message.Message);

			if (_eventSet.Contains(eventMessage.Metadata.MessageId))
				return;

			_eventSet.Add(eventMessage.Metadata);

			switch (eventMessage.Metadata.Type)
			{
				case EventSubMessageType.Notification:
					ResetKeepAliveTimer();
					await ProcessNotificationMessage(eventMessage.Metadata.SubscriptionType, message.Message);
					break;

				case EventSubMessageType.SessionWelcome:
					if (_reconnectCompleted is not null)
					{
						ResetKeepAliveTimer();
						await WebsocketReconnected(message.Message);
					}
					else
					{
						await ProcessSessionWelcomeMessage(message.Message);
					}
					break;

				case EventSubMessageType.SessionReconnect:
					await ProcessReconnectMessage(eventMessage.Payload.Session.ReconnectUrl);
					break;

				case EventSubMessageType.SessionKeepalive:
					ResetKeepAliveTimer();
					break;

				case EventSubMessageType.Revocation:
					await Signalr.SendError($"TwitchEventSub Revoked {eventMessage.Payload.Subscription.Type}");
					// TODO: Should this attempt to reconnect eventsub?
					break;

				default:
					throw new InvalidMetadataMessageTypeException("Unsupported message type.");
			}
		}
		catch (Exception e)
		{
			await Signalr.SendError(e.Message);
		}
	}

	private async Task WebsocketReconnected(string message)
	{
		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		sessionId = eventMessage.Payload.Session.Id;
		lock (_reconnectLock)
		{
			_reconnectCompleted?.SetResult();
		}
	}


	private async Task ProcessReconnectMessage(string reconnectUrl)
	{
		await Signalr.SendLog("TwitchEventSub reconnecting Websocket session");

		lock (_reconnectLock)
		{
			if (_reconnectCompleted is not null)
				return; // already connecting

			_reconnectCompleted = new TaskCompletionSource();
		}

		var oldFactory = Factory;
		var oldClient = TwitchEventSub;

		var newFactory = new WebSocketFactory();
		var newTwitchEventSub = await newFactory.Create(reconnectUrl, 3, ProcessMessage, Error, Closed);

		await _reconnectCompleted.Task;
		await Signalr.SendLog($"TwitchEventSub Websocket session reconnected {sessionId}");

		lock (_reconnectLock)
		{
			Factory = newFactory;
			TwitchEventSub = newTwitchEventSub;
			_reconnectCompleted = null;
		}

		if (oldFactory is not null)
		{
			await oldFactory.Disconnect();
			await Signalr.SendLog("TwitchEventSub old Factory session disconnected");
		}

		if (oldClient is not null)
		{
			await Signalr.SendLog("TwitchEventSub old Websocket session disconnected");
		}

	}

	private async Task Error(WebSocketMessage message)
	{
		Timer.Stop();

		Log.LogError("Websocket error: {message}", message);
		await Signalr.SendError(message.Message);
		if (TwitchEventSub?.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}

		await Task.Delay(TimeSpan.FromSeconds(5));
		await CreateWebSocket();
	}

	private async Task Closed(WebSocketMessage message)
	{
		Timer.Stop();

		// When websocket is closed, does not send event message
		Log.LogInformation($"TwitchEventSub Websocket closed: {message.Message}");
		if (TwitchEventSub?.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}

		await CreateWebSocket();
	}

	public async Task DisconnectWebSocket()
	{
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, Status.Offline);
		await Factory.Disconnect();
		await Signalr.SendLog("TwitchEventSub Websocket disconnected");
	}

	private async Task ProcessSessionWelcomeMessage(string message)
	{
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, Status.Loading);
		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		sessionId = eventMessage.Payload.Session.Id;

		if (_useCli) // Testing
		{
			await Signalr.SendInfo($"TwitchEventSub Websocket connected {sessionId}");
			return;
		}

		var eventsToSubscribeTo = Settings.Value.DebugMode
			? TwitchApiHelper.DebugSubscribeToEvents()
			: TwitchApiHelper.SubscribeToEvents();

		var requests = eventsToSubscribeTo.Select
			(x => CreateEventSubRequest(x, sessionId)).ToList();

		await TwitchApiRequest.CreateEventSubSubscription(requests);

		_timeoutSeconds = eventMessage.Payload.Session.KeepAliveTimeoutSeconds;
		StartKeepaliveTimer();
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, Status.Online);
		await Signalr.SendInfo($"TwitchEventSub Websocket connected {sessionId}");
	}


	private async Task ProcessNotificationMessage(EventSubSubscriptionType type, string message)
	{
		try
		{
			using var scope = ScopeFactory.CreateScope();

			switch (type)
			{
				case EventSubSubscriptionType.StreamOnline:
					var streamOnline = JsonSerializer.Deserialize<EventMessage<StreamOnlineEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IStreamOnlineHandler>()
						.Handle(streamOnline.Payload.Event);
					break;
				case EventSubSubscriptionType.StreamOffline:
					await scope.ServiceProvider.GetRequiredService<IStreamOfflineHandler>()
						.Handle();
					break;
				case EventSubSubscriptionType.ChannelUpdate:
					var channelUpdate = JsonSerializer.Deserialize<EventMessage<ChannelUpdatedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IStreamUpdatedHandler>()
						.Handle(channelUpdate.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelFollow:
					var follow = JsonSerializer.Deserialize<EventMessage<FollowEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IChannelFollowedHandler>()
						.Handle(follow.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelAdBreakBegin:
					var adBreakBegin = JsonSerializer.Deserialize<EventMessage<AdBreakBeginEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IAdBreakStartedHandler>()
						.Handle(adBreakBegin.Payload.Event);
					break;
				// case EventSubSubscriptionType.ChannelChatClear:
				// 	var chatCleared = JsonSerializer.Deserialize<EventMessage<ChatClearedEvent>>(message);
				// 	await Send(new ChatClearedCommand(chatCleared.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelChatClearUserMessages:
				// 	var userChatHistoryCleared = JsonSerializer.Deserialize<EventMessage<UserMessagesClearedEvent>>(message);
				// 	await Send(new UserChatHistoryClearedCommand(userChatHistoryCleared.Payload.Event));
				// 	break;
				case EventSubSubscriptionType.ChannelChatMessage:
					var chatMessageReceived = JsonSerializer.Deserialize<EventMessage<ChatMessageReceivedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IChatMessageReceivedHandler>()
						.Handle(chatMessageReceived.Payload.Event);
					break;
				// case EventSubSubscriptionType.ChannelChatMessageDelete:
				// 	var chatMessageDeleted = JsonSerializer.Deserialize<EventMessage<MessageDeletedEvent>>(message);
				// 	await Send(new ChatMessageDeletedCommand(chatMessageDeleted.Payload.Event));
				// 	break;
				case EventSubSubscriptionType.ChannelChatNotification:
					var chatNotificationReceived = JsonSerializer.Deserialize<EventMessage<ChatNotificationEvent>>(message);
					//if (args.Payload.Event.NoticeType == NoticeType.Announcement)
					//{
					//	// send to chat?
					//}
					if (chatNotificationReceived.Payload.Event.NoticeType == NoticeType.BitsBadgeTierUpgrade)
					{
						// Upgraded bit tier
					}

					//if (args.Payload.Event.NoticeType == NoticeType.CharityDonation)
					//{
					//	// send to chat?
					//}
					//if (args.Payload.Event.NoticeType == NoticeType.Raid)
					//{
					//	// send to chat?
					//}
					if (chatNotificationReceived.Payload.Event.NoticeType is
					    NoticeType.Sub or
					    NoticeType.Resub or
					    NoticeType.SubGift or
					    NoticeType.CommunitySubGift or
					    NoticeType.GiftSubPaidUpgrade or
					    NoticeType.PrimeSubPaidUpgrade or
					    NoticeType.PayItForwardSub
					   )
					{
						await scope.ServiceProvider.GetRequiredService<IChatNotificationReceivedEventHandler>()
							.Handle(chatNotificationReceived.Payload.Event);
					}

					break;
				// case EventSubSubscriptionType.ChannelSharedChatBegin:
				// break;
				// case EventSubSubscriptionType.ChannelSharedChatUpdate:
				// break;
				// case EventSubSubscriptionType.ChannelSharedChatEnd:
				// break;

				// case EventSubSubscriptionType.ChannelChatSettingsUpdate:
				// 	var chatSettingsUpdated = JsonSerializer.Deserialize<EventMessage<ChatSettingsUpdatedEvent>>(message);
				// 	await Send(new ChatSettingsUpdatedCommand(chatSettingsUpdated.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelSubscribe:
				// 	var subscriptionReceived = JsonSerializer.Deserialize<EventMessage<SubscriptionEvent>>(message);
				// 	await Send(new SubscriptionReceivedCommand(subscriptionReceived.Payload.Event));
				// 	break;
				//case SubscriptionType.ChannelSubscriptionEnd:
				//	OnChannelSubscriptionEnd?.Invoke(eventMessage);
				//	break;
				// case EventSubSubscriptionType.ChannelSubscriptionGift:
				// 	var subGifted = JsonSerializer.Deserialize<EventMessage<GiftedSubEvent>>(message);
				// 	await Send(new GiftSubReceivedCommand(subGifted.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelSubscriptionMessage:
				// 	var resub = JsonSerializer.Deserialize<EventMessage<ResubMessageEvent>>(message);
				// 	await Send(new ResubReceivedCommand(resub.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelCheer:
				// 	var cheer = JsonSerializer.Deserialize<EventMessage<CheerReceivedEvent>>(message);
				// 	await Send(new CheerReceivedCommand(cheer.Payload.Event));
				// 	break;
				case EventSubSubscriptionType.ChannelBitsUsed:
					var bitsUsed = JsonSerializer.Deserialize<EventMessage<BitsUsedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IBitEventReceivedHandler>()
						.Handle(bitsUsed.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelRaid:
					var raid = JsonSerializer.Deserialize<EventMessage<RaidEvent>>(message);
					if (raid.Payload.Event.ToBroadcasterId == "98683749")
					{
						await scope.ServiceProvider.GetRequiredService<IAddIncomingRaidHandler>()
							.Handle(raid.Payload.Event);
					}
					// else
					// {
					// 	await Send(new OutgoingRaidCommand(raid.Payload.Event));
					// }
				
					break;
				// case EventSubSubscriptionType.ChannelBan:
				// 	var userBanned = JsonSerializer.Deserialize<EventMessage<BannedUserEvent>>(message);
				// 	await Send(new UserBannedCommand(userBanned.Payload.Event));
				// 	break;
				//case EventSubSubscriptionType.ChannelUnban:
				//	var userUnbanned = JsonSerializer.Deserialize<EventMessage<UnbannedUserEvent>>(message);
				//	await Send(new UserUnbannedCommand(userUnbanned.Payload.Event));
				// break;
				//case EventSubSubscriptionType.ChannelModeratorAdd:
				//	var moderatorAdded = JsonSerializer.Deserialize<EventMessage<ModAddedEvent>>(message);
				//	await Send(new ModeratorAddedCommand(moderatorAdded.Payload.Event));
				//	break;
				//case EventSubSubscriptionType.ChannelModeratorRemove:
				//	var moderatorRemoved = JsonSerializer.Deserialize<EventMessage<ModRemovedEvent>>(message);
				//	await Send(new ModeratorRemovedCommand(moderatorRemoved.Payload.Event));
				//	break;

				//case SubscriptionType.ChannelModerate:
				//	OnModeratorAction?.Invoke(eventMessage);
				//	break;
				case EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption:
					var autoReward = JsonSerializer.Deserialize<EventMessage<AutomaticRewardRedemptionEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IAutoRewardRedeemedHandler>()
						.Handle(autoReward.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelPointsCustomRewardAdd:
					var customRewardCreated = JsonSerializer.Deserialize<EventMessage<CustomRewardCreatedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IPointRewardCreatedHandler>()
						.Handle(customRewardCreated.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelPointsCustomRewardUpdate:
					var customRewardUpdated = JsonSerializer.Deserialize<EventMessage<CustomRewardUpdatedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IPointRewardUpdatedHandler>()
						.Handle(customRewardUpdated.Payload.Event);
					break;
				// case EventSubSubscriptionType.ChannelPointsCustomRewardRemove:
				// 	var customRewardDeleted = JsonSerializer.Deserialize<EventMessage<CustomRewardRemovedEvent>>(message);
				// 	await Send(new PointRewardDeletedCommand(customRewardDeleted.Payload.Event));
				// 	break;
				case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd:
					var rewardRedemptionAdded = JsonSerializer.Deserialize<EventMessage<RewardRedemptionAddedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IRewardRedeemedHandler>()
						.Handle(rewardRedemptionAdded.Payload.Event);
					break;
				// case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate:
				// 	var rewardRedemptionUpdated = JsonSerializer.Deserialize<EventMessage<RewardRedemptionUpdatedEvent>>(message);
				// 	await scope.ServiceProvider.GetRequiredService<IRewardRedeemUpdateHandler>()
				// 		.Handle(rewardRedemptionUpdated.Payload.Event);
				// 	break;
				case EventSubSubscriptionType.ChannelPollBegin:
					var pollStarted = JsonSerializer.Deserialize<EventMessage<PollBeginEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IPollStartedHandler>()
						.Handle(pollStarted.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelPollProgress:
					var pollProgress = JsonSerializer.Deserialize<EventMessage<PollProgressEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IVoteReceivedHandler>()
						.Handle(pollProgress.Payload.Event);
					break;
				case EventSubSubscriptionType.ChannelPollEnd:
					var pollEnded = JsonSerializer.Deserialize<EventMessage<PollEndedEvent>>(message);
					await scope.ServiceProvider.GetRequiredService<IPollEndedHandler>()
						.Handle(pollEnded.Payload.Event);
					break;
				// case EventSubSubscriptionType.ChannelPredictionBegin:
				// 	var predictionStarted = JsonSerializer.Deserialize<EventMessage<PredictionBeginEvent>>(message);
				// 	await Send(new PredictionStartedCommand(predictionStarted.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelPredictionProgress:
				// 	var predictionProgressed = JsonSerializer.Deserialize<EventMessage<PredictionProgressEvent>>(message);
				// 	await Send(new PredictionReceivedCommand(predictionProgressed.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelPredictionLock:
				// 	var predictionLocked = JsonSerializer.Deserialize<EventMessage<PredictionLockedEvent>>(message);
				// 	await Send(new PredictionLockedCommand(predictionLocked.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelPredictionEnd:
				// 	var predictionEnded = JsonSerializer.Deserialize<EventMessage<PredictionEndedEvent>>(message);
				// 	await Send(new PredictionEndedCommand(predictionEnded.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelSuspiciousUserMessage:
				// 	var suspiciousUserAlert = JsonSerializer.Deserialize<EventMessage<SuspiciousUserMessageEvent>>(message);
				// 	await Send(new SuspiciousUserAlertCommand(suspiciousUserAlert.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelSuspiciousUserUpdate:
				// 	var suspicousUserUpdated = JsonSerializer.Deserialize<EventMessage<SuspiciousUserUpdatedEvent>>(message);
				// 	await Send(new SuspiciousUserUpdatedCommand(suspicousUserUpdated.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelVipAdd:
				// 	var vipAdded = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				// 	await Send(new VipAddedCommand(vipAdded.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelVipRemove:
				// 	var vipRemoved = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				// 	await Send(new VipRemovedCommand(vipRemoved.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.UserWarningAcknowledgement:
				// 	var userWarningAcknowledged = JsonSerializer.Deserialize<EventMessage<UserWarningAcknowledgedEvent>>(message);
				// 	await Send(new UserWarningAcknowledgedCommand(userWarningAcknowledged.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelWarningSend:
				// 	var userWarningSent = JsonSerializer.Deserialize<EventMessage<UserWarningSentEvent>>(message);
				// 	await Send(new UserWarningSentCommand(userWarningSent.Payload.Event));
				// 	break;
				//case SubscriptionType.CharityDonation:
				//	OnCharityDonation?.Invoke(eventMessage);
				//	break;
				//case SubscriptionType.CharityCampaignStart:
				//	OnCharityCampaignStart?.Invoke(eventMessage);
				//	break;
				//case SubscriptionType.CharityCampaignProgress:
				//	OnCharityCampaignProgress?.Invoke(eventMessage);
				//	break;
				//case SubscriptionType.CharityCampaignStop:
				//	OnCharityCampaignStop?.Invoke(eventMessage);
				//	break;
				// case EventSubSubscriptionType.ChannelGoalBegin:
				// 	var channelGoalStarted = JsonSerializer.Deserialize<EventMessage<ChannelGoalStartedEvent>>(message);
				// 	await Send(new ChannelGoalStartedCommand(channelGoalStarted.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelGoalProgress:
				// 	var channelGoalProgress = JsonSerializer.Deserialize<EventMessage<ChannelGoalProgressEvent>>(message);
				// 	await Send(new ChannelGoalProgressCommand(channelGoalProgress.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ChannelGoalEnd:
				// 	var channelGoalEnded = JsonSerializer.Deserialize<EventMessage<ChannelGoalEndedEvent>>(message);
				// 	await Send(new ChannelGoalEndedCommand(channelGoalEnded.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.HypeTrainBegin:
				// 	var hypeTrainStarted = JsonSerializer.Deserialize<EventMessage<HypeTrainStartedEvent>>(message);
				// 	await Send(new HypeTrainStartedCommand(hypeTrainStarted.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.HypeTrainProgress:
				// 	var hypeTrainProgressed = JsonSerializer.Deserialize<EventMessage<HypeTrainProgressedEvent>>(message);
				// 	await Send(new HypeTrainProgressedCommand(hypeTrainProgressed.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.HypeTrainEnd:
				// 	var hypeTrainEnded = JsonSerializer.Deserialize<EventMessage<HypeTrainEndedEvent>>(message);
				// 	await Send(new HypeTrainEndedCommand(hypeTrainEnded.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ShieldModeBegin:
				// 	var shieldModeStarted = JsonSerializer.Deserialize<EventMessage<ShieldModeBeginEvent>>(message);
				// 	await Send(new ShieldModeStartedCommand(shieldModeStarted.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ShieldModeEnd:
				// 	var shieldModeEnded = JsonSerializer.Deserialize<EventMessage<ShieldModeEndedEvent>>(message);
				// 	await Send(new ShieldModeEndedCommand(shieldModeEnded.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ShoutoutCreate:
				// 	var shoutoutCreated = JsonSerializer.Deserialize<EventMessage<ShoutoutCreatedEvent>>(message);
				// 	await Send(new ShoutoutSentCommand(shoutoutCreated.Payload.Event));
				// 	break;
				// case EventSubSubscriptionType.ShoutoutReceive:
				// 	var shoutoutReceived = JsonSerializer.Deserialize<EventMessage<ShoutoutReceivedEvent>>(message);
				// 	await Send(new ShoutoutReceivedCommand(shoutoutReceived.Payload.Event));
				// 	break;
				//case SubscriptionType.WhisperReceived:
				//	OnWhisperReceived?.Invoke(eventMessage);
				//	break;
				case EventSubSubscriptionType.NotSupported:
					throw new SubscriptionEventNotSupportedException("Subscription event not supported.");
				default:
					// create new exception type here
					throw new InvalidSubscriptionTypeException($"Invalid subscription type: {type}");
			}
		}
		catch (Exception ex)
		{
			Log.LogInformation($"{ex}");
		}
	}
	

	private async void TimerElapsed(object? sender, ElapsedEventArgs e)
	{
		try
		{
			await DisconnectWebSocket();
			await CreateWebSocket();
		}
		catch (Exception ex)
		{
			Log.LogError(ex, "Error in TimerElapsed event");
		}
	}

	private void StartKeepaliveTimer()
	{
		Timer.Stop();
		Timer.Start();
	}

	private void ResetKeepAliveTimer()
	{
		Timer.Stop();
		Timer.Start();
	}

	private CreateEventSubSubscriptionRequestBody CreateEventSubRequest
		(EventSubSubscriptionType type, string sessionId)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		return new CreateEventSubSubscriptionRequestBody
		{
			SubscriptionType = type,
			Version = type.GetVersion(),
			Condition = type.GetConditions(streamerId),
			Transport = new TransportMethod { SessionId = sessionId },
		};
	}

	public void SetCancellationToken(CancellationToken cancel)
		=> Cancel = cancel;
	public void Deconstruct(out IAppCache Cache, out IOptions<Settings> Settings, out IServiceScopeFactory ScopeFactory, out ISignalrService Signalr, out ILogger<TwitchEventSubService> Log, out ITwitchApiRequest TwitchApiRequest)
	{
		Cache = this.Cache;
		Settings = this.Settings;
		ScopeFactory = this.ScopeFactory;
		Signalr = this.Signalr;
		Log = this.Log;
		TwitchApiRequest = this.TwitchApiRequest;
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface ITwitchEventSubService
{
	void SetCancellationToken(CancellationToken cancel);
	Task CreateWebSocket();
	Task DisconnectWebSocket();
}