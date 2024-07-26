using Koishibot.Core.Services.Twitch.EventSubs.Exceptions;
using Koishibot.Core.Services.Twitch.EventSubs.RequestModels;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.AdBreak;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;
using Koishibot.Core.Services.TwitchEventSubNew.Enums;
using Koishibot.Core.Services.TwitchEventSubNew.Misc;
using Koishibot.Core.Services.Websockets;
using System.Text.Json;
using Timer = System.Timers.Timer;
namespace Koishibot.Core.Services.Twitch.EventSubs;



public class TwitchEventSubWebSocket : WebSocketHandlerBase
{
	public TwitchEventSubWebSocket(
			string url,
			CancellationToken cancellationToken,
			byte maxReconnectAttempts,
			List<SubscriptionType> eventSubs,
			ITwitchEventSubClient subClient
			) : base(url, cancellationToken, maxReconnectAttempts)
	{
		_client = subClient;
		MessageReceived += OnMessageReceived;
		_eventSubs = eventSubs;
	}

	private readonly LimitedSizeHashSet<Metadata, string> _eventSet = new(25, x => x.MessageId);
	private List<SubscriptionType> _eventSubs;
	private string _authToken;
	private string _sessionId = string.Empty;
	private int _keepaliveTimeoutSeconds;
	private Timer _keepaliveTimer;
	private ITwitchEventSubClient _client;


	//public Action<EventMessage>? OnStreamOnline { get; set; }
	//public Action<EventMessage>? OnStreamOffline { get; set; }
	public Action<EventMessage<ChannelUpdatedEvent>>? OnChannelUpdate { get; set; }
	//public Action<EventMessage>? OnChannelFollow { get; set; }
	public Action<EventMessage<AdBreakBeginEvent>>? OnChannelAdBreakBegin { get; set; }
	//public Action<EventMessage>? OnChannelChatClear { get; set; }
	//public Action<EventMessage>? OnChannelChatClearUserMessages { get; set; }
	//public Action<EventMessage>? OnChannelChatMessage { get; set; }
	//public Action<EventMessage>? OnChannelChatMessageDelete { get; set; }
	//public Action<EventMessage>? OnChannelChatNotification { get; set; }
	//public Action<EventMessage>? OnChannelChatSettingsUpdate { get; set; }
	//public Action<EventMessage>? OnChannelSubscribe { get; set; }
	//public Action<EventMessage>? OnChannelSubscriptionEnd { get; set; }
	//public Action<EventMessage>? OnChannelSubscriptionGift { get; set; }
	//public Action<EventMessage>? OnResubscriber { get; set; }
	//public Action<EventMessage>? OnChannelCheer { get; set; }
	//public Action<EventMessage>? OnChannelRaid { get; set; }
	//public Action<EventMessage>? OnChannelBan { get; set; }
	//public Action<EventMessage>? OnModeratorAction { get; set; }
	//public Action<EventMessage>? OnPowerUpRedeemed { get; set; }
	//public Action<EventMessage>? OnChannelPointsCustomRewardCreated { get; set; }
	//public Action<EventMessage>? OnChannelPointsCustomRewardUpdated { get; set; }
	//public Action<EventMessage>? OnChannelPointsCustomRewardRemoved { get; set; }
	//public Action<EventMessage>? OnChannelPointsCustomRewardRedemptionAdded { get; set; }
	//public Action<EventMessage>? OnChannelPointsCustomRewardRedemptionUpdated { get; set; }
	//public Action<EventMessage>? OnChannelPollBegin { get; set; }
	//public Action<EventMessage>? OnChannelPollProgress { get; set; }
	//public Action<EventMessage>? OnChannelPollEnd { get; set; }
	//public Action<EventMessage>? OnChannelPredictionBegin { get; set; }
	//public Action<EventMessage>? OnChannelPredictionProgress { get; set; }
	//public Action<EventMessage>? OnChannelPredictionLock { get; set; }
	//public Action<EventMessage>? OnChannelPredictionEnd { get; set; }
	//public Action<EventMessage>? OnChannelSuspiciousUserMessage { get; set; }
	//public Action<EventMessage>? OnChannelSuspiciousUserUpdate { get; set; }
	//public Action<EventMessage>? OnChannelVipAdd { get; set; }
	//public Action<EventMessage>? OnChannelVipRemove { get; set; }
	//public Action<EventMessage>? OnChannelWarningAcknowledgement { get; set; }
	//public Action<EventMessage>? OnChannelWarningSend { get; set; }
	//public Action<EventMessage>? OnCharityDonation { get; set; }
	//public Action<EventMessage>? OnCharityCampaignStart { get; set; }
	//public Action<EventMessage>? OnCharityCampaignProgress { get; set; }
	//public Action<EventMessage>? OnCharityCampaignStop { get; set; }
	//public Action<EventMessage>? OnGoalBegin { get; set; }
	//public Action<EventMessage>? OnGoalProgress { get; set; }
	//public Action<EventMessage>? OnGoalEnd { get; set; }
	//public Action<EventMessage>? OnHypeTrainBegin { get; set; }
	//public Action<EventMessage>? OnHypeTrainProgress { get; set; }
	//public Action<EventMessage>? OnHypeTrainEnd { get; set; }
	//public Action<EventMessage>? OnShieldModeBegin { get; set; }
	//public Action<EventMessage>? OnShieldModeEnd { get; set; }
	//public Action<EventMessage>? OnShoutoutCreate { get; set; }
	//public Action<EventMessage>? OnShoutoutReceived { get; set; }
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

		switch (eventMessage.Metadata.MessageType)
		{
			case MessageType.Notification:
				ProcessNotificationMessage(eventMessage.Metadata.SubscriptionType, message);
				break;
			case MessageType.SessionWelcome:
				await ProcessSessionWelcomeMessage(message);
				break;
			case MessageType.SessionReconnect:
				break;
			case MessageType.SessionKeepalive:
				OnKeepAliveMessage?.Invoke(eventMessage.Metadata.MessageId);
				break;
			case MessageType.Revocation:
				break;
			default:
				throw new InvalidMetadataMessageTypeException("Unsupported message type.");
		}
	}

	private async Task ProcessSessionWelcomeMessage(string message)
	{
		var eventMessage = JsonSerializer.Deserialize<EventMessage<object>>(message);
		var response = eventMessage.Payload.Session.Id;

		await _client.SubscribeToEvents(_eventSubs, response);

		_keepaliveTimeoutSeconds = eventMessage.Payload.Session.KeepAliveTimeoutSeconds;
		StartKeepaliveTimer();
		OnWelcomeMessage?.Invoke(response);
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

	private void ProcessNotificationMessage(SubscriptionType type, string message)
	{
		switch (type)
		{
			//case SubscriptionType.StreamOnline:
			//	OnStreamOnline?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.StreamOffline:
			//	OnStreamOffline?.Invoke(eventMessage);
			//	break;
			case SubscriptionType.ChannelUpdate:
				var channelUpdate = JsonSerializer.Deserialize<EventMessage<ChannelUpdatedEvent>>(message);
				OnChannelUpdate?.Invoke(channelUpdate!);
				break;
			//case SubscriptionType.ChannelFollow:
			//	OnChannelFollow?.Invoke(eventMessage);
			//	break;
			case SubscriptionType.ChannelAdBreakBegin:
				var adBreakBegin = JsonSerializer.Deserialize<EventMessage<AdBreakBeginEvent>>(message);
				OnChannelAdBreakBegin?.Invoke(adBreakBegin!);
				break;
			//case SubscriptionType.ChannelChatClear:
			//	OnChannelChatClear?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelChatClearUserMessages:
			//	OnChannelChatClearUserMessages?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelChatMessage:
			//	OnChannelChatMessage?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelChatMessageDelete:
			//	OnChannelChatMessageDelete?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelChatNotification:
			//	OnChannelChatNotification?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelChatSettingsUpdate:
			//	OnChannelChatSettingsUpdate?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSubscribe:
			//	OnChannelSubscribe?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSubscriptionEnd:
			//	OnChannelSubscriptionEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSubscriptionGift:
			//	OnChannelSubscriptionGift?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSubscriptionMessage:
			//	OnResubscriber?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelCheer:
			//	OnChannelCheer?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelRaid:
			//	OnChannelRaid?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelBan:
			//	OnChannelBan?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelModerate:
			//	OnModeratorAction?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPointsAutomaticRewardRedemption:
			//	OnPowerUpRedeemed?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPointsCustomRewardAdd:
			//	OnChannelPointsCustomRewardCreated?.Invoke(eventMessage);
			//	break;	
			//case SubscriptionType.ChannelPointsCustomRewardUpdate:
			//	OnChannelPointsCustomRewardUpdated?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPointsCustomRewardRemove:
			//	OnChannelPointsCustomRewardRemoved?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPointsCustomRewardRedemptionAdd:
			//	OnChannelPointsCustomRewardRedemptionAdded?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPointsCustomRewardRedemptionUpdate:
			//	OnChannelPointsCustomRewardRedemptionUpdated?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPollBegin:
			//	OnChannelPollBegin?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPollProgress:
			//	OnChannelPollProgress?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPollEnd:
			//	OnChannelPollEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPredictionBegin:
			//	OnChannelPredictionBegin?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPredictionProgress:
			//	OnChannelPredictionProgress?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPredictionLock:
			//	OnChannelPredictionLock?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelPredictionEnd:
			//	OnChannelPredictionEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSuspiciousUserMessage:
			//	OnChannelSuspiciousUserMessage?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelSuspiciousUserUpdate:
			//	OnChannelSuspiciousUserUpdate?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelVipAdd:
			//	OnChannelVipAdd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelVipRemove:
			//	OnChannelVipRemove?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelWarningAcknowledgement:
			//	OnChannelWarningAcknowledgement?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ChannelWarningSend:
			//	OnChannelWarningSend?.Invoke(eventMessage);
			//	break;
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
			//case SubscriptionType.GoalBegin:
			//	OnGoalBegin?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.GoalProgress:
			//	OnGoalProgress?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.GoalEnd:
			//	OnGoalEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.HypeTrainBegin:
			//	OnHypeTrainBegin?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.HypeTrainProgress:
			//	OnHypeTrainProgress?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.HypeTrainEnd:
			//	OnHypeTrainEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ShieldModeBegin:
			//	OnShieldModeBegin?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ShieldModeEnd:
			//	OnShieldModeEnd?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ShoutoutCreate:
			//	OnShoutoutCreate?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.ShoutoutReceived:
			//	OnShoutoutReceived?.Invoke(eventMessage);
			//	break;
			//case SubscriptionType.WhisperReceived:
			//	OnWhisperReceived?.Invoke(eventMessage);
			//	break;
			case SubscriptionType.NotSupported:
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
			var rightNow = DateTime.UtcNow;
			var lastEvent = _eventSet.LastItem();
			if (rightNow.Subtract(lastEvent.MessageTimestamp).Seconds < _keepaliveTimeoutSeconds - 3)
			{
				return;
			}
			await Disconnect();
			await StartEventSubscriptions();
		};
		_keepaliveTimer.Start();
	}
}
