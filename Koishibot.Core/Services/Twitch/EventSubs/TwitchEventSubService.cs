using System.Diagnostics;
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
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;
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
using Timer = System.Timers.Timer;

namespace Koishibot.Core.Services.Twitch.EventSubs;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record TwitchEventSubService(
IAppCache Cache,
IOptions<Settings> Settings,
IServiceScopeFactory ScopeFactory,
ISignalrService SignalrService,
ILogger<TwitchEventSubService> Log,
ITwitchApiRequest TwitchApiRequest
) : ITwitchEventSubService
{
	public CancellationToken? Cancel { get; set; }

	private WebSocketFactory Factory { get; set; } = new();
	private WebSocketHandler? TwitchEventSub { get; set; }
	private int _timeoutSeconds = 60;
	private Timer Timer { get; } = new(TimeSpan.FromSeconds(63));
	private bool _useCli = false; // For testing on debug

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
			Log.LogError("An error has occured: {e}", e);
			await SignalrService.SendError(e.Message);
			throw new CustomException(e.Message);
		}
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		// if message is event notification or keep alive, reset timer
		// if timer has elapsed, reconnect
		try
		{
			if (TwitchEventSub is null)
			{
				return;
			}

			if (message.IsPing())
			{
				await TwitchEventSub.SendMessage("PONG");
				return;
			}

			var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message.Message);
			if (eventMessage == null)
				throw new JsonDeserializeException(message.Message);

			if (!_eventSet.Contains(eventMessage.Metadata.MessageId))
			{
				_eventSet.Add(eventMessage.Metadata);
			}

			switch (eventMessage.Metadata.Type)
			{
				case EventSubMessageType.Notification:
					Log.LogInformation($"Twitch Notification {eventMessage.Metadata.Timestamp}");
					OnEventReceived();
					await ProcessNotificationMessage(eventMessage.Metadata.SubscriptionType, message.Message);
					break;
				case EventSubMessageType.SessionWelcome:
					StartKeepaliveTimer();
					await ProcessSessionWelcomeMessage(message.Message);
					break;
				case EventSubMessageType.SessionReconnect:
					Log.LogInformation($"TwitchEventSub Reconnect Session {eventMessage.Payload.Session.ReconnectUrl}");
					break;
				case EventSubMessageType.SessionKeepalive:
					// Log.LogInformation($"TwitchEventSub Keepalive {eventMessage.Metadata.Timestamp}");
					OnEventReceived();
					//OnKeepAliveMessage?.Invoke(eventMessage.Metadata.MessageId);
					break;
				case EventSubMessageType.Revocation:
					Log.LogInformation($"TwitchEventSub Revoked {eventMessage.Payload.Subscription.Type}");
					break;
				default:
					throw new InvalidMetadataMessageTypeException("Unsupported message type.");
			}
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			Log.LogError(e, "An error has occured");
			await SignalrService.SendError(e.Message);
		}
	}

	private async Task Error(WebSocketMessage message)
	{
		Log.LogError("Websocket error: {message}", message);
		await SignalrService.SendError(message.Message);
		if (TwitchEventSub is not null && TwitchEventSub.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}
	}

	private async Task Closed(WebSocketMessage message)
	{
		Log.LogInformation($"Websocket closed {message}");
		if (TwitchEventSub is not null && TwitchEventSub.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}
	}

	public async Task DisconnectWebSocket()
	{
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, ServiceStatusString.Offline);
		await Factory.Disconnect();
		await SignalrService.SendInfo("TwitchEventSub Websocket Disconnected");
	}


	private async Task ProcessSessionWelcomeMessage(string message)
	{
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, ServiceStatusString.Loading);
		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		var sessionId = eventMessage.Payload.Session.Id;

		if (_useCli) return; // Testing

		var eventsToSubscribeTo = Settings.Value.DebugMode
		? TwitchApiHelper.DebugSubscribeToEvents()
		: TwitchApiHelper.SubscribeToEvents();

		var requests = eventsToSubscribeTo.Select
		(x => CreateEventSubRequest(x, sessionId)).ToList();

		await TwitchApiRequest.CreateEventSubSubscription(requests);

		_timeoutSeconds = eventMessage.Payload.Session.KeepAliveTimeoutSeconds;
		StartKeepaliveTimer();
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, ServiceStatusString.Online);
		await SignalrService.SendInfo("TwitchEventSub Websocket Connected");
	}


	private async Task ProcessNotificationMessage(EventSubSubscriptionType type, string message)
	{
		switch (type)
		{
			case EventSubSubscriptionType.StreamOnline:
				var streamOnline = JsonSerializer.Deserialize<EventMessage<StreamOnlineEvent>>(message);
				await Send(new StreamOnlineCommand());
				break;
			case EventSubSubscriptionType.StreamOffline:
				var streamOffline = JsonSerializer.Deserialize<EventMessage<StreamOfflineEvent>>(message);
				await Send(new StreamOfflineCommand());
				break;
			case EventSubSubscriptionType.ChannelUpdate:
				var channelUpdate = JsonSerializer.Deserialize<EventMessage<ChannelUpdatedEvent>>(message);
				await Send(new StreamUpdatedCommand(channelUpdate.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelFollow:
				var follow = JsonSerializer.Deserialize<EventMessage<FollowEvent>>(message);
				await Send(new ChannelFollowedCommand(follow.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelAdBreakBegin:
				var adBreakBegin = JsonSerializer.Deserialize<EventMessage<AdBreakBeginEvent>>(message);
				await Send(new AdBreakStartedCommand(adBreakBegin.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelChatClear:
				var chatCleared = JsonSerializer.Deserialize<EventMessage<ChatClearedEvent>>(message);
				await Send(new ChatClearedCommand(chatCleared.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelChatClearUserMessages:
				var userChatHistoryCleared = JsonSerializer.Deserialize<EventMessage<UserMessagesClearedEvent>>(message);
				await Send(new UserChatHistoryClearedCommand(userChatHistoryCleared.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelChatMessage:
				var chatMessageReceived = JsonSerializer.Deserialize<EventMessage<ChatMessageReceivedEvent>>(message);
				await Send(new ChatMessageReceivedCommand(chatMessageReceived.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelChatMessageDelete:
				var chatMessageDeleted = JsonSerializer.Deserialize<EventMessage<MessageDeletedEvent>>(message);
				await Send(new ChatMessageDeletedCommand(chatMessageDeleted.Payload.Event));
				break;
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
				if (chatNotificationReceived.Payload.Event.NoticeType == NoticeType.Sub ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.Resub ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.SubGift ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.CommunitySubGift ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.GiftSubPaidUpgrade ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.PrimeSubPaidUpgrade ||
				    chatNotificationReceived.Payload.Event.NoticeType == NoticeType.PayItForwardSub
				   )
				{
					// send to chat?
				}

				break;
			case EventSubSubscriptionType.ChannelChatSettingsUpdate:
				var chatSettingsUpdated = JsonSerializer.Deserialize<EventMessage<ChatSettingsUpdatedEvent>>(message);
				await Send(new ChatSettingsUpdatedCommand(chatSettingsUpdated.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelSubscribe:
				var subscriptionReceived = JsonSerializer.Deserialize<EventMessage<SubscriptionEvent>>(message);
				await Send(new SubscriptionReceivedCommand(subscriptionReceived.Payload.Event));
				break;
			//case SubscriptionType.ChannelSubscriptionEnd:
			//	OnChannelSubscriptionEnd?.Invoke(eventMessage);
			//	break;
			case EventSubSubscriptionType.ChannelSubscriptionGift:
				var subGifted = JsonSerializer.Deserialize<EventMessage<GiftedSubEvent>>(message);
				await Send(new GiftSubReceivedCommand(subGifted.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelSubscriptionMessage:
				var resub = JsonSerializer.Deserialize<EventMessage<ResubMessageEvent>>(message);
				await Send(new ResubReceivedCommand(resub.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelCheer:
				var cheer = JsonSerializer.Deserialize<EventMessage<CheerReceivedEvent>>(message);
				await Send(new CheerReceivedCommand(cheer.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelRaid:
				var raid = JsonSerializer.Deserialize<EventMessage<RaidEvent>>(message);
				if (raid.Payload.Event.ToBroadcasterId == "98683749")
				{
					await Send(new IncomingRaidCommand(raid.Payload.Event));
				}
				else
				{
					await Send(new OutgoingRaidCommand(raid.Payload.Event));
				}

				break;
			case EventSubSubscriptionType.ChannelBan:
				var userBanned = JsonSerializer.Deserialize<EventMessage<BannedUserEvent>>(message);
				await Send(new UserBannedCommand(userBanned.Payload.Event));
				break;
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
				var powerup = JsonSerializer.Deserialize<EventMessage<AutomaticRewardRedemptionEvent>>(message);
				await Send(new AutoRewardRedeemedCommand(powerup.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardAdd:
				var customRewardCreated = JsonSerializer.Deserialize<EventMessage<CustomRewardCreatedEvent>>(message);
				await Send(new PointRewardCreatedCommand(customRewardCreated.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardUpdate:
				var customRewardUpdated = JsonSerializer.Deserialize<EventMessage<CustomRewardUpdatedEvent>>(message);
				await Send(new PointRewardUpdatedCommand(customRewardUpdated.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRemove:
				var customRewardDeleted = JsonSerializer.Deserialize<EventMessage<CustomRewardRemovedEvent>>(message);
				await Send(new PointRewardDeletedCommand(customRewardDeleted.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd:
				var rewardRedemptionAdded = JsonSerializer.Deserialize<EventMessage<RewardRedemptionAddedEvent>>(message);
				await Send(new RedeemedRewardCommand(rewardRedemptionAdded.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate:
				var rewardRedemptionUpdated = JsonSerializer.Deserialize<EventMessage<RewardRedemptionUpdatedEvent>>(message);
				await Send(new RedeemedRewardUpdatedCommand(rewardRedemptionUpdated.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPollBegin:
				var pollStarted = JsonSerializer.Deserialize<EventMessage<PollBeginEvent>>(message);
				await Send(new PollStartedCommand(pollStarted.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPollProgress:
				var pollProgress = JsonSerializer.Deserialize<EventMessage<PollProgressEvent>>(message);
				await Send(new PollVoteReceivedCommand(pollProgress.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPollEnd:
				var pollEnded = JsonSerializer.Deserialize<EventMessage<PollEndedEvent>>(message);
				await Send(new PollEndedCommand(pollEnded.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPredictionBegin:
				var predictionStarted = JsonSerializer.Deserialize<EventMessage<PredictionBeginEvent>>(message);
				await Send(new PredictionStartedCommand(predictionStarted.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPredictionProgress:
				var predictionProgressed = JsonSerializer.Deserialize<EventMessage<PredictionProgressEvent>>(message);
				await Send(new PredictionReceivedCommand(predictionProgressed.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPredictionLock:
				var predictionLocked = JsonSerializer.Deserialize<EventMessage<PredictionLockedEvent>>(message);
				await Send(new PredictionLockedCommand(predictionLocked.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelPredictionEnd:
				var predictionEnded = JsonSerializer.Deserialize<EventMessage<PredictionEndedEvent>>(message);
				await Send(new PredictionEndedCommand(predictionEnded.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelSuspiciousUserMessage:
				var suspiciousUserAlert = JsonSerializer.Deserialize<EventMessage<SuspiciousUserMessageEvent>>(message);
				await Send(new SuspiciousUserAlertCommand(suspiciousUserAlert.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelSuspiciousUserUpdate:
				var suspicousUserUpdated = JsonSerializer.Deserialize<EventMessage<SuspiciousUserUpdatedEvent>>(message);
				await Send(new SuspiciousUserUpdatedCommand(suspicousUserUpdated.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelVipAdd:
				var vipAdded = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				await Send(new VipAddedCommand(vipAdded.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelVipRemove:
				var vipRemoved = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				await Send(new VipRemovedCommand(vipRemoved.Payload.Event));
				break;
			case EventSubSubscriptionType.UserWarningAcknowledgement:
				var userWarningAcknowledged = JsonSerializer.Deserialize<EventMessage<UserWarningAcknowledgedEvent>>(message);
				await Send(new UserWarningAcknowledgedCommand(userWarningAcknowledged.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelWarningSend:
				var userWarningSent = JsonSerializer.Deserialize<EventMessage<UserWarningSentEvent>>(message);
				await Send(new UserWarningSentCommand(userWarningSent.Payload.Event));
				break;
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
			case EventSubSubscriptionType.ChannelGoalBegin:
				var channelGoalStarted = JsonSerializer.Deserialize<EventMessage<ChannelGoalStartedEvent>>(message);
				await Send(new ChannelGoalStartedCommand(channelGoalStarted.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelGoalProgress:
				var channelGoalProgress = JsonSerializer.Deserialize<EventMessage<ChannelGoalProgressEvent>>(message);
				await Send(new ChannelGoalProgressCommand(channelGoalProgress.Payload.Event));
				break;
			case EventSubSubscriptionType.ChannelGoalEnd:
				var channelGoalEnded = JsonSerializer.Deserialize<EventMessage<ChannelGoalEndedEvent>>(message);
				await Send(new ChannelGoalEndedCommand(channelGoalEnded.Payload.Event));
				break;
			case EventSubSubscriptionType.HypeTrainBegin:
				var hypeTrainStarted = JsonSerializer.Deserialize<EventMessage<HypeTrainStartedEvent>>(message);
				await Send(new HypeTrainStartedCommand(hypeTrainStarted.Payload.Event));
				break;
			case EventSubSubscriptionType.HypeTrainProgress:
				var hypeTrainProgressed = JsonSerializer.Deserialize<EventMessage<HypeTrainProgressedEvent>>(message);
				await Send(new HypeTrainProgressedCommand(hypeTrainProgressed.Payload.Event));
				break;
			case EventSubSubscriptionType.HypeTrainEnd:
				var hypeTrainEnded = JsonSerializer.Deserialize<EventMessage<HypeTrainEndedEvent>>(message);
				await Send(new HypeTrainEndedCommand(hypeTrainEnded.Payload.Event));
				break;
			case EventSubSubscriptionType.ShieldModeBegin:
				var shieldModeStarted = JsonSerializer.Deserialize<EventMessage<ShieldModeBeginEvent>>(message);
				await Send(new ShieldModeStartedCommand(shieldModeStarted.Payload.Event));
				break;
			case EventSubSubscriptionType.ShieldModeEnd:
				var shieldModeEnded = JsonSerializer.Deserialize<EventMessage<ShieldModeEndedEvent>>(message);
				await Send(new ShieldModeEndedCommand(shieldModeEnded.Payload.Event));
				break;
			case EventSubSubscriptionType.ShoutoutCreate:
				var shoutoutCreated = JsonSerializer.Deserialize<EventMessage<ShoutoutCreatedEvent>>(message);
				await Send(new ShoutoutSentCommand(shoutoutCreated.Payload.Event));
				break;
			case EventSubSubscriptionType.ShoutoutReceive:
				var shoutoutReceived = JsonSerializer.Deserialize<EventMessage<ShoutoutReceivedEvent>>(message);
				await Send(new ShoutoutReceivedCommand(shoutoutReceived.Payload.Event));
				break;
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

	public async Task Send<T>(T args)
	{
		try
		{
			using var scope = ScopeFactory.CreateScope();
			var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

			await mediatr.Send(args);
		}
		catch (Exception ex)
		{
			Log.LogInformation($"{ex}");
		}
	}

	private void StartKeepaliveTimer()
	{
		Timer.Elapsed += async (_, _) =>
		{
			await DisconnectWebSocket();
			await CreateWebSocket();
		};

		Timer.Start();
	}

	private void OnEventReceived()
	{
		Timer.Stop();
		Timer.Start();
	}

	public CreateEventSubSubscriptionRequestBody CreateEventSubRequest
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
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface ITwitchEventSubService
{
	void SetCancellationToken(CancellationToken cancel);
	Task CreateWebSocket();
	Task DisconnectWebSocket();
}