using Koishibot.Core.Features.AdBreak.Events;
using Koishibot.Core.Features.StreamInformation.Events;
using Koishibot.Core.Services.TwitchEventSubNew.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs;

public record TwitchEventSubService(
		IOptions<Settings> Settings,
		IServiceScopeFactory ScopeFactory,
		ISignalrService SignalrService,
		ILogger<TwitchEventSubService> Log,
		ITwitchEventSubClient SubClient
		) : ITwitchEventSubService
{
	public CancellationToken? Cancel { get; set; }
	public TwitchEventSubWebSocket? TwitchEventSub { get; set; }

	public List<SubscriptionType> EventsToSubscribeTo { get; set; } = [];

	private const byte KeepAliveSeconds = 60;



	public async Task CreateWebSocket(CancellationToken cancel)
	{
		SubscribeToEvents();

		Cancel ??= cancel;
		TwitchEventSub ??= new TwitchEventSubWebSocket(
						$"wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds={KeepAliveSeconds}",
						Cancel.Value, 3, EventsToSubscribeTo, SubClient);

		SetupEventSubHandlerEvents();

		await TwitchEventSub.StartEventSubscriptions();
	}

	public void SubscribeToEvents()
	{
		EventsToSubscribeTo.AddRange(new[]
		{
			//SubscriptionType.AutomodMessageHold,
			//SubscriptionType.AutomodMessageUpdate,
			//SubscriptionType.AutomodSettingsUpdate,
			//SubscriptionType.AutomodTermsUpdate,
			SubscriptionType.ChannelUpdate,
			SubscriptionType.ChannelFollow,
			SubscriptionType.ChannelAdBreakBegin,
			//SubscriptionType.ChannelChatClear,
			//SubscriptionType.ChannelChatClearUserMessages,
			//SubscriptionType.ChannelChatMessage,
			//SubscriptionType.ChannelChatMessageDelete,
			SubscriptionType.ChannelChatNotification,
			//SubscriptionType.ChannelChatSettingsUpdate,
			//SubscriptionType.ChannelChatUserMessageHold,
			//SubscriptionType.ChannelChatUserMessageUpdate,
			SubscriptionType.ChannelSubscribe,
			SubscriptionType.ChannelSubscriptionEnd,
			SubscriptionType.ChannelSubscriptionGift,
			SubscriptionType.ChannelSubscriptionMessage,
			SubscriptionType.ChannelCheer,
			SubscriptionType.ChannelRaidSent,
			SubscriptionType.ChannelRaidReceived,
			SubscriptionType.ChannelBan,
			SubscriptionType.ChannelUnban,
			//SubscriptionType.ChannelUnbanRequestCreate,
			//SubscriptionType.ChannelUnbanRequestResolve,
			SubscriptionType.ChannelModerate,
			SubscriptionType.ChannelModeratorAdd,
			SubscriptionType.ChannelModeratorRemove,
			//SubscriptionType.ChannelGuestStarSessionBegin,
			//SubscriptionType.ChannelGuestStarSessionEnd,
			//SubscriptionType.ChannelGuestStarGuestUpdate,
			//SubscriptionType.ChannelGuestStarSettingsUpdate,
			SubscriptionType.ChannelPointsAutomaticRewardRedemption,
			SubscriptionType.ChannelPointsCustomRewardAdd,
			SubscriptionType.ChannelPointsCustomRewardUpdate,
			SubscriptionType.ChannelPointsCustomRewardRemove,
			SubscriptionType.ChannelPointsCustomRewardRedemptionAdd,
			SubscriptionType.ChannelPointsCustomRewardRedemptionUpdate,
			SubscriptionType.ChannelPollBegin,
			SubscriptionType.ChannelPollProgress,
			SubscriptionType.ChannelPollEnd,
			SubscriptionType.ChannelPredictionBegin,
			SubscriptionType.ChannelPredictionProgress,
			SubscriptionType.ChannelPredictionLock,
			SubscriptionType.ChannelPredictionEnd,
			SubscriptionType.ChannelSuspiciousUserMessage,
			SubscriptionType.ChannelSuspiciousUserUpdate,
			SubscriptionType.ChannelVipAdd,
			SubscriptionType.ChannelVipRemove,
			SubscriptionType.ChannelWarningAcknowledgement,
			SubscriptionType.ChannelWarningSend,
			//SubscriptionType.CharityDonation,
			//SubscriptionType.CharityCampaignStart,
			//SubscriptionType.CharityCampaignProgress,
			//SubscriptionType.CharityCampaignStop,
			//SubscriptionType.ConduitShardDisabled,
			//SubscriptionType.DropEntitlementGrant,
			//SubscriptionType.ExtensionBitsTransactionCreate,
			SubscriptionType.GoalBegin,
			SubscriptionType.GoalProgress,
			SubscriptionType.GoalEnd,
			SubscriptionType.HypeTrainBegin,
			SubscriptionType.HypeTrainProgress,
			SubscriptionType.HypeTrainEnd,
			SubscriptionType.ShieldModeBegin,
			SubscriptionType.ShieldModeEnd,
			SubscriptionType.ShoutoutCreate,
			SubscriptionType.ShoutoutReceive,
			SubscriptionType.StreamOnline,
			SubscriptionType.StreamOffline,
			//SubscriptionType.UserAuthorizationGrant,
			//SubscriptionType.UserAuthorizationRevoke,
			//SubscriptionType.UserUpdate,
			//SubscriptionType.WhisperReceived,
		});
	}


	public async void SetupEventSubHandlerEvents()
	{
		if (TwitchEventSub is null) { return; }

		//TwitchEventSub.OnWelcomeMessage += async message =>


		TwitchEventSub.OnChannelUpdate += async args =>
			await Send(new StreamUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelAdBreakBegin += async args =>
			await Send(new AdBreakStartedCommand(args.Payload.Event));

	}

	public async Task Send<T>(T args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(args);
	}
}

public interface ITwitchEventSubService
{
	Task CreateWebSocket(CancellationToken cancel);
}