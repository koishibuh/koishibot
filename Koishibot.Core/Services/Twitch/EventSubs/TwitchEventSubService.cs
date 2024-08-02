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
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Services.Twitch.EventSubs;

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
	public TwitchEventSubWebSocket? TwitchEventSub { get; set; }

	public List<EventSubSubscriptionType> EventsToSubscribeTo { get; set; } = [];

	private const byte KeepAliveSeconds = 60;

	public async Task CreateWebSocket(CancellationToken cancel)
	{
		SubscribeToEvents();

		Cancel ??= cancel;
		TwitchEventSub ??= new TwitchEventSubWebSocket(
			$"wss://eventsub.wss.twitch.tv/ws?keepalive_timeout_seconds={KeepAliveSeconds}",
			Settings, Cancel.Value, 3, EventsToSubscribeTo, TwitchApiRequest);

		SetupEventSubHandlerEvents();

		await TwitchEventSub.StartEventSubscriptions();

	}

	public void SubscribeToEvents()
	{
		EventsToSubscribeTo.AddRange(new[]
		{
			//EventSubSubscriptionType.AutomodMessageHold,
			//EventSubSubscriptionType.AutomodMessageUpdate,
			//EventSubSubscriptionType.AutomodSettingsUpdate,
			//EventSubSubscriptionType.AutomodTermsUpdate,
			EventSubSubscriptionType.ChannelUpdate,
			EventSubSubscriptionType.ChannelFollow,
			EventSubSubscriptionType.ChannelAdBreakBegin,
			//EventSubSubscriptionType.ChannelChatClear,
			//EventSubSubscriptionType.ChannelChatClearUserMessages,
			EventSubSubscriptionType.ChannelChatMessage,
			EventSubSubscriptionType.ChannelChatMessageDelete,
			EventSubSubscriptionType.ChannelChatNotification,
			//EventSubSubscriptionType.ChannelChatSettingsUpdate,
			//EventSubSubscriptionType.ChannelChatUserMessageHold,
			//EventSubSubscriptionType.ChannelChatUserMessageUpdate,
			EventSubSubscriptionType.ChannelSubscribe,
			EventSubSubscriptionType.ChannelSubscriptionEnd,
			EventSubSubscriptionType.ChannelSubscriptionGift,
			EventSubSubscriptionType.ChannelSubscriptionMessage,
			EventSubSubscriptionType.ChannelCheer,
			EventSubSubscriptionType.ChannelRaidSent,
			EventSubSubscriptionType.ChannelRaidReceived,
			EventSubSubscriptionType.ChannelBan,
			EventSubSubscriptionType.ChannelUnban,
			//EventSubSubscriptionType.ChannelUnbanRequestCreate,
			//EventSubSubscriptionType.ChannelUnbanRequestResolve,
			EventSubSubscriptionType.ChannelModerate,
			EventSubSubscriptionType.ChannelModeratorAdd,
			EventSubSubscriptionType.ChannelModeratorRemove,
			//EventSubSubscriptionType.ChannelGuestStarSessionBegin,
			//EventSubSubscriptionType.ChannelGuestStarSessionEnd,
			//EventSubSubscriptionType.ChannelGuestStarGuestUpdate,
			//EventSubSubscriptionType.ChannelGuestStarSettingsUpdate,
			EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption,
			EventSubSubscriptionType.ChannelPointsCustomRewardAdd,
			EventSubSubscriptionType.ChannelPointsCustomRewardUpdate,
			EventSubSubscriptionType.ChannelPointsCustomRewardRemove,
			EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd,
			EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate,
			EventSubSubscriptionType.ChannelPollBegin,
			EventSubSubscriptionType.ChannelPollProgress,
			EventSubSubscriptionType.ChannelPollEnd,
			EventSubSubscriptionType.ChannelPredictionBegin,
			EventSubSubscriptionType.ChannelPredictionProgress,
			EventSubSubscriptionType.ChannelPredictionLock,
			EventSubSubscriptionType.ChannelPredictionEnd,
			EventSubSubscriptionType.ChannelSuspiciousUserMessage,
			EventSubSubscriptionType.ChannelSuspiciousUserUpdate,
			EventSubSubscriptionType.ChannelVipAdd,
			EventSubSubscriptionType.ChannelVipRemove,
			EventSubSubscriptionType.UserWarningAcknowledgement,
			EventSubSubscriptionType.ChannelWarningSend,
			//EventSubSubscriptionType.CharityDonation,
			//EventSubSubscriptionType.CharityCampaignStart,
			//EventSubSubscriptionType.CharityCampaignProgress,
			//EventSubSubscriptionType.CharityCampaignStop,
			//EventSubSubscriptionType.ConduitShardDisabled,
			//EventSubSubscriptionType.DropEntitlementGrant,
			//EventSubSubscriptionType.ExtensionBitsTransactionCreate,
			EventSubSubscriptionType.ChannelGoalBegin,
			EventSubSubscriptionType.ChannelGoalProgress,
			EventSubSubscriptionType.ChannelGoalEnd,
			EventSubSubscriptionType.HypeTrainBegin,
			EventSubSubscriptionType.HypeTrainProgress,
			EventSubSubscriptionType.HypeTrainEnd,
			EventSubSubscriptionType.ShieldModeBegin,
			EventSubSubscriptionType.ShieldModeEnd,
			EventSubSubscriptionType.ShoutoutCreate,
			EventSubSubscriptionType.ShoutoutReceive,
			EventSubSubscriptionType.StreamOnline,
			EventSubSubscriptionType.StreamOffline,
			//EventSubSubscriptionType.UserAuthorizationGrant,
			//EventSubSubscriptionType.UserAuthorizationRevoke,
			//EventSubSubscriptionType.UserUpdate,
			//EventSubSubscriptionType.WhisperReceived,
		});
	}


	public async void SetupEventSubHandlerEvents()
	{
		if (TwitchEventSub is null) { return; }

		TwitchEventSub.OnWelcomeMessage += async message =>
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, true);

		TwitchEventSub.OnStreamUpdated += async args =>
		await Send(new StreamUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelFollow += async args =>
			await Send(new ChannelFollowedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelAdBreakBegin += async args =>
			await Send(new AdBreakStartedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelChatClear += async args =>
			await Send(new ChatClearedCommand(args.Payload.Event));

		TwitchEventSub.OnChatMessageReceived += async args =>
		await Send(new ChatMessageReceivedCommand(args.Payload.Event));


		TwitchEventSub.OnChatMessageDelete += async args =>
		await Send(new ChatMessageDeletedCommand(args.Payload.Event));

		TwitchEventSub.OnUserMessagesCleared += async args =>
		await Send(new UserChatHistoryClearedCommand(args.Payload.Event));

		TwitchEventSub.OnChatNotification += async args =>
		{
			//if (args.Payload.Event.NoticeType == NoticeType.Announcement)
			//{
			//	// send to chat?
			//}
			if (args.Payload.Event.NoticeType == NoticeType.BitsBadgeTierUpgrade)
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
			if (args.Payload.Event.NoticeType == NoticeType.Sub ||
			args.Payload.Event.NoticeType == NoticeType.Resub ||
			args.Payload.Event.NoticeType == NoticeType.SubGift ||
			args.Payload.Event.NoticeType == NoticeType.CommunitySubGift ||
			args.Payload.Event.NoticeType == NoticeType.GiftSubPaidUpgrade ||
			args.Payload.Event.NoticeType == NoticeType.PrimeSubPaidUpgrade ||
			args.Payload.Event.NoticeType == NoticeType.PayItForwardSub
			)
			{
				// send to chat?
			}



		};

		TwitchEventSub.OnChatSettingsUpdated += async args =>
		await Send(new ChatSettingsUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnNewSubscription += async args =>
			await Send(new SubscriptionReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnSubGifted += async args =>
			await Send(new GiftSubReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnResubscription += async args =>
			await Send(new ResubReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnCheerReceived += async args =>
			await Send(new CheerReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelRaid += async args =>
		{
			if (args.Payload.Event.ToBroadcasterId == "98683749")
			{
				await Send(new IncomingRaidCommand(args.Payload.Event));
			}
			else
			{
				await Send(new OutgoingRaidCommand(args.Payload.Event));
			}
		};

		TwitchEventSub.OnUserBanned += async args =>
			await Send(new UserBannedCommand(args.Payload.Event));

		TwitchEventSub.OnUserUnbanned += async args =>
		await Send(new UserUnbannedCommand(args.Payload.Event));

		TwitchEventSub.OnModeratorAdded += async args =>
		await Send(new ModeratorAddedCommand (args.Payload.Event));

		TwitchEventSub.OnModeratorRemoved += async args =>
		await Send(new ModeratorRemovedCommand(args.Payload.Event));

		TwitchEventSub.OnPowerUpRedeemed += async args =>
			await Send(new AutoRewardRedeemedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPointsCustomRewardCreated += async args =>
			await Send(new PointRewardCreatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPointsCustomRewardUpdated += async args =>
			await Send(new PointRewardUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPointsCustomRewardRemoved += async args =>
		await Send(new PointRewardDeletedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPointsCustomRewardRedemptionAdded += async args =>
			await Send(new RedeemedRewardCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPointsCustomRewardRedemptionUpdated += async args =>
		await Send(new RedeemedRewardUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPollStarted += async args =>
		await Send(new PollStartedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPollVoteReceived += async args =>
			await Send(new PollVoteReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelPollEnded += async args =>
			await Send(new PollEndedCommand(args.Payload.Event));

		TwitchEventSub.OnPredictionStarted += async args =>
		await Send(new PredictionStartedCommand(args.Payload.Event));

		TwitchEventSub.OnPredictionReceived += async args =>
		await Send(new PredictionReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnPredictionLocked += async args =>
		await Send(new PredictionLockedCommand(args.Payload.Event));

		TwitchEventSub.OnPredictionEnded += async args =>
		await Send(new PredictionEndedCommand(args.Payload.Event));

		TwitchEventSub.OnSuspiciousUserMessage += async args =>
		await Send(new SuspiciousUserAlertCommand(args.Payload.Event));

		TwitchEventSub.OnSuspiciousUserUpdated += async args =>
			await Send(new SuspiciousUserUpdatedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelVipAdded += async args =>
			await Send(new VipAddedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelVipRemoved += async args =>
			await Send(new VipRemovedCommand(args.Payload.Event));

		TwitchEventSub.OnUserWarningAcknowledgemented += async args =>
		await Send(new UserWarningAcknowledgedCommand(args.Payload.Event));

		TwitchEventSub.OnUserWarningSent += async args =>
		await Send(new UserWarningSentCommand(args.Payload.Event));

		TwitchEventSub.OnChannelGoalStarted += async args =>
		await Send(new ChannelGoalStartedCommand(args.Payload.Event));

		TwitchEventSub.OnChannelGoalProgress += async args =>
		await Send(new ChannelGoalProgressCommand(args.Payload.Event));

		TwitchEventSub.OnChannelGoalEnded += async args =>
		await Send(new ChannelGoalEndedCommand(args.Payload.Event));

		TwitchEventSub.OnHypeTrainStarted += async args =>
			await Send(new HypeTrainStartedCommand(args.Payload.Event));

		TwitchEventSub.OnHypeTrainProgressed += async args =>
			await Send(new HypeTrainProgressedCommand(args.Payload.Event));

		TwitchEventSub.OnHypeTrainEnded += async args =>
		await Send(new HypeTrainEndedCommand(args.Payload.Event));

		TwitchEventSub.OnShieldModeStarted += async args =>
			await Send(new ShieldModeStartedCommand(args.Payload.Event));

		TwitchEventSub.OnShieldModeEnded += async args =>
			await Send(new ShieldModeEndedCommand(args.Payload.Event));

		TwitchEventSub.OnShoutoutReceived += async args =>
		await Send(new ShoutoutReceivedCommand(args.Payload.Event));

		TwitchEventSub.OnStreamOnline += async args =>
		await Send(new StreamOnlineCommand());

		TwitchEventSub.OnStreamOffline += async args =>
		await Send(new StreamOfflineCommand());

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