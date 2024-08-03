using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatSettings;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderator;
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
using Timer = System.Timers.Timer;
namespace Koishibot.Core.Services.Twitch.EventSubs;

public class TwitchEventSubWebSocket : WebSocketHandlerBase
{
	public TwitchEventSubWebSocket(
		string url,
		IOptions<Settings> Settings,
		CancellationToken cancellationToken,
		byte maxReconnectAttempts,
		List<EventSubSubscriptionType> eventSubs,
		ITwitchApiRequest twitchApiRequest
	) : base(url, cancellationToken, maxReconnectAttempts)
	{
		_settings = Settings;
		_twitchApiRequest = twitchApiRequest;
		MessageReceived += OnMessageReceived;
		_eventSubs = eventSubs;
	}

	private readonly LimitedSizeHashSet<Metadata, string> _eventSet = new(25, x => x.MessageId);
	private List<EventSubSubscriptionType> _eventSubs;
	private string _authToken;
	private string _sessionId = string.Empty;
	private int _keepaliveTimeoutSeconds;
	private Timer _keepaliveTimer;
	private ITwitchApiRequest _twitchApiRequest;
	private IOptions<Settings> _settings;

	//public Action <EventMessage<AutomodMessageHoldEvent>? OnAutomodMessageHeld { get; set; }
	//public Action <EventMessage<AutoModMessageUpdateEvent>? OnAutomodMessageUpdated { get; set; }
	//public Action <EventMessage<AutomodSettingsUpdateEvent>? OnAutomodSettingsUpdated { get; set; }
	//public Action <EventMessage<AutomodTermsUpdateEvent>? OnAutomodTermsUpdated { get; set; }
	public Action<EventMessage<ChannelUpdatedEvent>>? OnStreamUpdated { get; set; }
	public Action<EventMessage<FollowEvent>>? OnChannelFollow { get; set; }
	public Action<EventMessage<AdBreakBeginEvent>>? OnChannelAdBreakBegin { get; set; }
	public Action<EventMessage<ChatClearedEvent>>? OnChannelChatClear { get; set; }
	public Action<EventMessage<UserMessagesClearedEvent>>? OnUserMessagesCleared { get; set; }
	public Action<EventMessage<ChatMessageReceivedEvent>>? OnChatMessageReceived { get; set; }
	public Action<EventMessage<MessageDeletedEvent>>? OnChatMessageDelete { get; set; }
	public Action<EventMessage<ChatNotificationEvent>>? OnChatNotification { get; set; }
	public Action<EventMessage<ChatSettingsUpdatedEvent>>? OnChatSettingsUpdated { get; set; }
	//public Action<EventMessage<UserMessageHoldEvent>>? OnUserMessageHeld { get; set; }
	//public Action<EventMessage<UserMessageUpdateEvent>>? OnUserMessageUpdated { get; set; }
	public Action<EventMessage<SubscriptionEvent>>? OnNewSubscription { get; set; }
	//public Action<EventMessage>? OnChannelSubscriptionEnd { get; set; }
	public Action<EventMessage<GiftedSubEvent>>? OnSubGifted { get; set; }
	public Action<EventMessage<ResubMessageEvent>>? OnResubscription { get; set; }
	public Action<EventMessage<CheerReceivedEvent>>? OnCheerReceived { get; set; }
	public Action<EventMessage<RaidEvent>>? OnChannelRaid { get; set; }
	public Action<EventMessage<BannedUserEvent>>? OnUserBanned { get; set; }
	public Action<EventMessage<UnbannedUserEvent>>? OnUserUnbanned { get; set; }
	//public Action<EventMessage<UnbanRequestCreatedEvent>>? OnUnbanRequestCreated { get; set; }
	//public Action<EventMessage<UnbanRequestResolvedEvent>>? OnUnbannedRequestResolved { get; set; }
	//public Action<EventMessage>? OnModeratorAction { get; set; }
	public Action<EventMessage<ModAddedEvent>>? OnModeratorAdded {get; set;}
	public Action<EventMessage<ModRemovedEvent>>? OnModeratorRemoved {get; set;}
	//public Action<EventMessage<GuestStarSessionBeginEvent>>? OnGuestStarSessionStarted {get; set;}
	//public Action<EventMessage<GuestStarSessionEndedEvent>>? OnGuestStarSessionEnded {get; set;}
	//public Action<EventMessage<GuestStarGuestUpdateEvent>>? OnGuestStarGuestUpdate {get; set;}
	//public Action<EventMessage<GuestStarSettingsUpdateEvent>>? OnGuestStarSettingsUpdated {get; set;}
	public Action<EventMessage<AutomaticRewardRedemptionEvent>>? OnPowerUpRedeemed { get; set; }
	public Action<EventMessage<CustomRewardCreatedEvent>>? OnChannelPointsCustomRewardCreated { get; set; }
	public Action<EventMessage<CustomRewardUpdatedEvent>>? OnChannelPointsCustomRewardUpdated { get; set; }
	public Action<EventMessage<CustomRewardRemovedEvent>>? OnChannelPointsCustomRewardRemoved { get; set; }
	public Action<EventMessage<RewardRedemptionAddedEvent>>? OnChannelPointsCustomRewardRedemptionAdded { get; set; }
	public Action<EventMessage<RewardRedemptionUpdatedEvent>>? OnChannelPointsCustomRewardRedemptionUpdated { get; set; }
	public Action<EventMessage<PollBeginEvent>>? OnChannelPollStarted { get; set; }
	public Action<EventMessage<PollProgressEvent>>? OnChannelPollVoteReceived { get; set; }
	public Action<EventMessage<PollEndedEvent>>? OnChannelPollEnded { get; set; }
	public Action<EventMessage<PredictionBeginEvent>>? OnPredictionStarted { get; set; }
	public Action<EventMessage<PredictionProgressEvent>>? OnPredictionReceived { get; set; }
	public Action<EventMessage<PredictionLockedEvent>>? OnPredictionLocked { get; set; }
	public Action<EventMessage<PredictionEndedEvent>>? OnPredictionEnded { get; set; }
	public Action<EventMessage<SuspiciousUserMessageEvent>>? OnSuspiciousUserMessage { get; set; }
	public Action<EventMessage<SuspiciousUserUpdatedEvent>>? OnSuspiciousUserUpdated { get; set; }
	public Action<EventMessage<VipEvent>>? OnChannelVipAdded { get; set; }
	public Action<EventMessage<VipEvent>>? OnChannelVipRemoved { get; set; }
	public Action<EventMessage<UserWarningAcknowledgedEvent>>? OnUserWarningAcknowledgemented { get; set; }
	public Action<EventMessage<UserWarningSentEvent>>? OnUserWarningSent { get; set; }
	//public Action<EventMessage>? OnCharityDonation { get; set; }
	//public Action<EventMessage>? OnCharityCampaignStart { get; set; }
	//public Action<EventMessage>? OnCharityCampaignProgress { get; set; }
	//public Action<EventMessage>? OnCharityCampaignStop { get; set; }
	public Action<EventMessage<ChannelGoalStartedEvent>>? OnChannelGoalStarted { get; set; }
	public Action<EventMessage<ChannelGoalProgressEvent>>? OnChannelGoalProgress { get; set; }
	public Action<EventMessage<ChannelGoalEndedEvent>>? OnChannelGoalEnded { get; set; }
	public Action<EventMessage<HypeTrainEvent>>? OnHypeTrainStarted { get; set; }
	public Action<EventMessage<HypeTrainEvent>>? OnHypeTrainProgressed { get; set; }
	public Action<EventMessage<HypeTrainEvent>>? OnHypeTrainEnded { get; set; }
	public Action<EventMessage<ShieldModeBeginEvent>>? OnShieldModeStarted { get; set; }
	public Action<EventMessage<ShieldModeEndedEvent>>? OnShieldModeEnded { get; set; }
	//public Action<EventMessage>? OnShoutoutCreate { get; set; }
	public Action<EventMessage<ShoutoutReceivedEvent>>? OnShoutoutReceived { get; set; }
	public Action<EventMessage<StreamOnlineEvent>>? OnStreamOnline { get; set; }
	public Action<EventMessage<StreamOfflineEvent>>? OnStreamOffline { get; set; }
	//public Action<EventMessage>? OnWhisperReceived { get; set; }

	public Action<string>? OnKeepAliveMessage { get; set; }
	public Action<string>? OnWelcomeMessage { get; set; }


	public async Task StartEventSubscriptions()
	{
		await Task.Run(async () => await Connect());
	}

	public void SetNewAuthToken(string authToken)
	{
		_authToken = authToken;
		//_twitchApiService = TwitchApiServiceFactory.CreateTwitchApiService(_authToken, _clientId, CancellationToken);
	}

	private async void OnMessageReceived(string message)
	{
		if (message.Contains("PING"))
		{
			await SendMessage("PONG");
			return;
		}

		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		if (eventMessage == null)
		{
			return;
		}

		if (!_eventSet.Contains(eventMessage.Metadata.MessageId))
		{
			_eventSet.Add(eventMessage.Metadata);
		}

		switch (eventMessage.Metadata.Type)
		{
			case EventSubMessageType.Notification:
				ProcessNotificationMessage(eventMessage.Metadata.SubscriptionType, message);
				break;
			case EventSubMessageType.SessionWelcome:
				await ProcessSessionWelcomeMessage(message);
				break;
			case EventSubMessageType.SessionReconnect:
				break;
			case EventSubMessageType.SessionKeepalive:
				OnKeepAliveMessage?.Invoke(eventMessage.Metadata.MessageId);
				break;
			case EventSubMessageType.Revocation:
				break;
			default:
				throw new InvalidMetadataMessageTypeException("Unsupported message type.");
		}
	}

	private async Task ProcessSessionWelcomeMessage(string message)
	{
		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		var sessionId = eventMessage.Payload.Session.Id;


		var requests = _eventSubs.Select
				(x => CreateEventSubRequest(x, sessionId)).ToList();

		//var eventSubRequestTasks = await requests.Select
		//		(request => _twitchApiRequest.CreateEventSubSubscription(requests)).ToList();

		await _twitchApiRequest.CreateEventSubSubscription(requests);
		

		//await Task.WhenAll(eventSubRequestTasks);

		_keepaliveTimeoutSeconds = eventMessage.Payload.Session.KeepAliveTimeoutSeconds;
		StartKeepaliveTimer();
		OnWelcomeMessage?.Invoke(sessionId);


		// original
		//var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		//var response = eventMessage.Payload.Session.Id;

		//await _client.SubscribeToEvents(_eventSubs, response);

		//_keepaliveTimeoutSeconds = eventMessage.Payload.Session.KeepAliveTimeoutSeconds;
		//StartKeepaliveTimer();
		//OnWelcomeMessage?.Invoke(response);




		// Create object for streamupdate

		//if (eventMessage.Payload is not null)
		//{
		//	_sessionId = eventMessage.Payload.Session.Id;
		//	var subscriptionTasks =
		//					_events.Select(eventType => _twitchApiService.SubscribeToEvents(new EventSubRequest
		//					{
		//						Type = eventType,
		//						Transport = new EventSubTransportRequest
		//						{
		//							SessionId = _sessionId
		//						},
		//						Condition = new Dictionary<string, string>
		//						{
		//							["broadcaster_user_id"] = _broadcasterId,
		//							["user_id"] = _userId
		//						},
		//						Version = 1
		//					})).ToList();
		//	await Task.WhenAll(subscriptionTasks);
		//	_keepaliveTimeoutSeconds = eventMessage.Payload.Session.KeepaliveTimeoutSeconds;
		//	StartKeepaliveTimer();
		//	OnWelcomeMessage?.Invoke(eventMessage);
		//}
	}

	public CreateEventSubSubscriptionRequestBody CreateEventSubRequest
			(EventSubSubscriptionType type, string sessionId)
	{
		var streamerId = _settings.Value.StreamerTokens.UserId;

		return new CreateEventSubSubscriptionRequestBody
		{
			SubscriptionType = type,
			Version = type.GetVersion(),
			Condition = type.GetConditions(streamerId),
			Transport = new TransportMethod { SessionId = sessionId },
		};
	}


	private void ProcessNotificationMessage(EventSubSubscriptionType type, string message)
	{
		switch (type)
		{
			case EventSubSubscriptionType.StreamOnline:
				var streamOnline = JsonSerializer.Deserialize<EventMessage<StreamOnlineEvent>>(message);
				OnStreamOnline?.Invoke(streamOnline);
				break;
			case EventSubSubscriptionType.StreamOffline:
				var streamOffline = JsonSerializer.Deserialize<EventMessage<StreamOfflineEvent>>(message);
				OnStreamOffline?.Invoke(streamOffline);
				break;
			case EventSubSubscriptionType.ChannelUpdate:
				var channelUpdate = JsonSerializer.Deserialize<EventMessage<ChannelUpdatedEvent>>(message);
				OnStreamUpdated?.Invoke(channelUpdate!);
				break;
			case EventSubSubscriptionType.ChannelFollow:
				var follow = JsonSerializer.Deserialize<EventMessage<FollowEvent>>(message);
				OnChannelFollow?.Invoke(follow);
				break;
			case EventSubSubscriptionType.ChannelAdBreakBegin:
				var adBreakBegin = JsonSerializer.Deserialize<EventMessage<AdBreakBeginEvent>>(message);
				OnChannelAdBreakBegin?.Invoke(adBreakBegin!);
				break;
			case EventSubSubscriptionType.ChannelChatClear:
				var chatCleared = JsonSerializer.Deserialize<EventMessage<ChatClearedEvent>>(message);
				OnChannelChatClear?.Invoke(chatCleared);
				break;
			case EventSubSubscriptionType.ChannelChatClearUserMessages:
				var userChatHistoryCleared = JsonSerializer.Deserialize<EventMessage<UserMessagesClearedEvent>>(message);
				OnUserMessagesCleared?.Invoke(userChatHistoryCleared);
				break;
			case EventSubSubscriptionType.ChannelChatMessage:
				var chatMessageReceived = JsonSerializer.Deserialize<EventMessage<ChatMessageReceivedEvent>>(message);
				OnChatMessageReceived?.Invoke(chatMessageReceived);
				break;
			case EventSubSubscriptionType.ChannelChatMessageDelete:
				var chatMessageDeleted = JsonSerializer.Deserialize<EventMessage<MessageDeletedEvent>>(message);
				OnChatMessageDelete?.Invoke(chatMessageDeleted);
				break;
			case EventSubSubscriptionType.ChannelChatNotification:
				var chatNotificationReceived = JsonSerializer.Deserialize<EventMessage<ChatNotificationEvent>>(message);
				OnChatNotification?.Invoke(chatNotificationReceived);
				break;
			case EventSubSubscriptionType.ChannelChatSettingsUpdate:
				var chatSettingsUpdated = JsonSerializer.Deserialize<EventMessage<ChatSettingsUpdatedEvent>>(message);
				OnChatSettingsUpdated?.Invoke(chatSettingsUpdated);
				break;
			case EventSubSubscriptionType.ChannelSubscribe:
				var subscriptionReceived = JsonSerializer.Deserialize<EventMessage<SubscriptionEvent>>(message);
				OnNewSubscription?.Invoke(subscriptionReceived);
				break;
			//case SubscriptionType.ChannelSubscriptionEnd:
			//	OnChannelSubscriptionEnd?.Invoke(eventMessage);
			//	break;
			case EventSubSubscriptionType.ChannelSubscriptionGift:
				var subGifted = JsonSerializer.Deserialize<EventMessage<GiftedSubEvent>>(message);
				OnSubGifted?.Invoke(subGifted);
				break;
			case EventSubSubscriptionType.ChannelSubscriptionMessage:
				var resub = JsonSerializer.Deserialize<EventMessage<ResubMessageEvent>>(message);
				OnResubscription?.Invoke(resub);
				break;
			case EventSubSubscriptionType.ChannelCheer:
				var cheer = JsonSerializer.Deserialize<EventMessage<CheerReceivedEvent>>(message);
				OnCheerReceived?.Invoke(cheer!);
				break;
			case EventSubSubscriptionType.ChannelRaid:
				var raid = JsonSerializer.Deserialize<EventMessage<RaidEvent>>(message);
				OnChannelRaid?.Invoke(raid!);
				break;
			case EventSubSubscriptionType.ChannelBan:
				var userBanned = JsonSerializer.Deserialize<EventMessage<BannedUserEvent>>(message);
				OnUserBanned?.Invoke(userBanned);
				break;
			//case SubscriptionType.ChannelModerate:
			//	OnModeratorAction?.Invoke(eventMessage);
			//	break;
			case EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption:
				var powerup = JsonSerializer.Deserialize<EventMessage<AutomaticRewardRedemptionEvent>>(message);
				OnPowerUpRedeemed?.Invoke(powerup);
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardAdd:
				var customRewardCreated = JsonSerializer.Deserialize<EventMessage<CustomRewardCreatedEvent>>(message);
				OnChannelPointsCustomRewardCreated?.Invoke(customRewardCreated!);
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardUpdate:
				var customRewardUpdated = JsonSerializer.Deserialize<EventMessage<CustomRewardUpdatedEvent>>(message);
				OnChannelPointsCustomRewardUpdated?.Invoke(customRewardUpdated!);
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRemove:
				var customRewardDeleted = JsonSerializer.Deserialize<EventMessage<CustomRewardRemovedEvent>>(message);
				OnChannelPointsCustomRewardRemoved?.Invoke(customRewardDeleted);
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd:
				var rewardRedemptionAdded = JsonSerializer.Deserialize<EventMessage<RewardRedemptionAddedEvent>>(message);
				OnChannelPointsCustomRewardRedemptionAdded?.Invoke(rewardRedemptionAdded!);
				break;
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate:
				var rewardRedemptionUpdated = JsonSerializer.Deserialize<EventMessage<RewardRedemptionUpdatedEvent>>(message);
				OnChannelPointsCustomRewardRedemptionUpdated?.Invoke(rewardRedemptionUpdated);
				break;
			case EventSubSubscriptionType.ChannelPollBegin:
				var pollStarted = JsonSerializer.Deserialize<EventMessage<PollBeginEvent>>(message);
				OnChannelPollStarted?.Invoke(pollStarted!);
				break;
			case EventSubSubscriptionType.ChannelPollProgress:
				var pollProgress = JsonSerializer.Deserialize<EventMessage<PollProgressEvent>>(message);
				OnChannelPollVoteReceived?.Invoke(pollProgress!);
				break;
			case EventSubSubscriptionType.ChannelPollEnd:
				var pollEnded = JsonSerializer.Deserialize<EventMessage<PollEndedEvent>>(message);
				OnChannelPollEnded?.Invoke(pollEnded!);
				break;
			case EventSubSubscriptionType.ChannelPredictionBegin:
				var predictionStarted = JsonSerializer.Deserialize<EventMessage<PredictionBeginEvent>>(message);
				OnPredictionStarted?.Invoke(predictionStarted);
				break;
			case EventSubSubscriptionType.ChannelPredictionProgress:
				var predictionProgressed = JsonSerializer.Deserialize<EventMessage<PredictionProgressEvent>>(message);
				OnPredictionReceived?.Invoke(predictionProgressed);
				break;
			case EventSubSubscriptionType.ChannelPredictionLock:
				var predictionLocked = JsonSerializer.Deserialize<EventMessage<PredictionLockedEvent>>(message);
				OnPredictionLocked?.Invoke(predictionLocked);
				break;
			case EventSubSubscriptionType.ChannelPredictionEnd:
				var predictionEnded = JsonSerializer.Deserialize<EventMessage<PredictionEndedEvent>>(message);
				OnPredictionEnded?.Invoke(predictionEnded);
				break;
			case EventSubSubscriptionType.ChannelSuspiciousUserMessage:
				var suspiciousUserAlert = JsonSerializer.Deserialize<EventMessage<SuspiciousUserMessageEvent>>(message);
				OnSuspiciousUserMessage?.Invoke(suspiciousUserAlert);
				break;
			case EventSubSubscriptionType.ChannelSuspiciousUserUpdate:
				var suspicousUserUpdated = JsonSerializer.Deserialize<EventMessage<SuspiciousUserUpdatedEvent>>(message);
				OnSuspiciousUserUpdated?.Invoke(suspicousUserUpdated);
				break;
			case EventSubSubscriptionType.ChannelVipAdd:
				var vipAdded = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				OnChannelVipAdded?.Invoke(vipAdded);
				break;
			case EventSubSubscriptionType.ChannelVipRemove:
				var vipRemoved = JsonSerializer.Deserialize<EventMessage<VipEvent>>(message);
				OnChannelVipRemoved?.Invoke(vipRemoved);
				break;
			case EventSubSubscriptionType.UserWarningAcknowledgement:
				var userWarningAcknowledged = JsonSerializer.Deserialize<EventMessage<UserWarningAcknowledgedEvent>>(message);
				OnUserWarningAcknowledgemented?.Invoke(userWarningAcknowledged);
				break;
			case EventSubSubscriptionType.ChannelWarningSend:
				var userWarningSent = JsonSerializer.Deserialize<EventMessage<UserWarningSentEvent>>(message);
				OnUserWarningSent?.Invoke(userWarningSent);
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
				OnChannelGoalStarted?.Invoke(channelGoalStarted);
				break;
			case EventSubSubscriptionType.ChannelGoalProgress:
				var channelGoalProgress  = JsonSerializer.Deserialize<EventMessage<ChannelGoalProgressEvent>>(message);
				OnChannelGoalProgress?.Invoke(channelGoalProgress);
				break;
			case EventSubSubscriptionType.ChannelGoalEnd:
				var channelGoalEnded  = JsonSerializer.Deserialize<EventMessage<ChannelGoalEndedEvent>>(message);
				OnChannelGoalEnded?.Invoke(channelGoalEnded);
				break;
			case EventSubSubscriptionType.HypeTrainBegin:
				var hypeTrainStarted = JsonSerializer.Deserialize<EventMessage<HypeTrainEvent>>(message);
				OnHypeTrainStarted?.Invoke(hypeTrainStarted);
				break;
			case EventSubSubscriptionType.HypeTrainProgress:
				var hypeTrainProgressed = JsonSerializer.Deserialize<EventMessage<HypeTrainEvent>>(message);
				OnHypeTrainProgressed?.Invoke(hypeTrainProgressed);
				break;
			case EventSubSubscriptionType.HypeTrainEnd:
				var hypeTrainEnded = JsonSerializer.Deserialize<EventMessage<HypeTrainEvent>>(message);
				OnHypeTrainEnded?.Invoke(hypeTrainEnded);
				break;
			case EventSubSubscriptionType.ShieldModeBegin:
				var shieldModeStarted = JsonSerializer.Deserialize<EventMessage<ShieldModeBeginEvent>>(message);
				OnShieldModeStarted?.Invoke(shieldModeStarted);
				break;
			case EventSubSubscriptionType.ShieldModeEnd:
				var shieldModeEnded = JsonSerializer.Deserialize<EventMessage<ShieldModeEndedEvent>>(message);
				OnShieldModeEnded?.Invoke(shieldModeEnded);
				break;
			//case SubscriptionType.ShoutoutCreate:
			//	OnShoutoutCreate?.Invoke(eventMessage);
			//	break;
			case EventSubSubscriptionType.ShoutoutReceive:
				var shoutoutReceived = JsonSerializer.Deserialize<EventMessage<ShoutoutReceivedEvent>>(message);
				OnShoutoutReceived?.Invoke(shoutoutReceived);
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

	private void StartKeepaliveTimer()
	{
		_keepaliveTimer = new Timer(TimeSpan.FromSeconds(_keepaliveTimeoutSeconds));
		_keepaliveTimer.Elapsed += async (_, _) =>
		{
			var rightNow = DateTimeOffset.UtcNow;
			var lastEvent = _eventSet.LastItem();
			if (rightNow.Subtract(lastEvent.Timestamp).Seconds < _keepaliveTimeoutSeconds - 3)
			{
				return;
			}
			await Disconnect();
			await StartEventSubscriptions();
		};
		_keepaliveTimer.Start();
	}
}
