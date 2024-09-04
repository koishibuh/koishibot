using System.Text.Json;
using Koishibot.Core.Services.Twitch.EventSubs;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.CharityCampaign;
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
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.User;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Vip;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;

namespace Koishibot.XTest;

public class TwitchEventSubTests
{
	[Fact]
	public void StreamOnlineEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/StreamStatus/stream-online.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<StreamOnlineEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void StreamOfflineEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/StreamStatus/stream-offline.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<StreamOfflineEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChannelUpdatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelUpdate/channelupdate-2.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChannelUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void FollowEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Follow/follow-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<FollowEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void AdBreakBegin()
	{
		var text = File.ReadAllText("../../../EventSubData/AdBreak/ad-break.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<AdBreakBeginEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChatClearedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/DeleteMessages/chat-cleared.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChatClearedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void UserMessagesClearedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/DeleteMessages/user-messages-cleared.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<UserMessagesClearedEvent>>(text);

		Assert.NotNull(data.Payload);
	}


	[Fact]
	public void ChatMessageReceivedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChatMessage/chat-message-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChatMessageReceivedEvent>>(text);

		Assert.NotNull(data.Payload);
	}


	[Fact]
	public void MessageDeletedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/DeleteMessages/chat-message-deleted.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<MessageDeletedEvent>>(text);

		Assert.NotNull(data.Payload);
	}


	[Fact]
	public void ChatNotificationEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChatNotification/chat-notification-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChatNotificationEvent>>(text);

		Assert.NotNull(data.Payload);
	}


	[Fact]
	public void ChannelChatSettingsUpdate()
	{
		var text = File.ReadAllText("../../../EventSubData/ChatSettings/chat-settings-updated.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChatSettingsUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void SubscriptionEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Subscriptions/subscription.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<SubscriptionEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void SubEndedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Subscriptions/subscription-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<SubEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void GiftedSubEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Subscriptions/subscription-gifted.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<GiftedSubEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ResubMessageEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Subscriptions/resubscription-message.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ResubMessageEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CheerReceivedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Cheers/cheer-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CheerReceivedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void RaidEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Raids/channelraid.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<RaidEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void BannedUserEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/BanUser/discipline-user.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<BannedUserEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void UnbannedUserEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/BanUser/unban-user.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<UnbannedUserEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ModAddedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Moderator/mod-added.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ModAddedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ModRemovedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Moderator/mod-removed.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ModRemovedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void AutomaticRewardRedemptionEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/autonatuc-reward-redemption-added.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<AutomaticRewardRedemptionEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CustomRewardCreatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/custom-reward-added.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CustomRewardCreatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CustomRewardUpdatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/custom-reward-updated.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CustomRewardUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CustomRewardRemovedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/ChannelPoints/custom-reward-removed.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CustomRewardRemovedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void RewardRedemptionAddedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/custom-reward-redemption-added.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<RewardRedemptionAddedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void RewardRedemptionUpdatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelPoints/custom-reward-redemption-updated.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<RewardRedemptionUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PollBeginEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Polls/poll-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PollBeginEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PollProgressEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Polls/poll-progress.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PollProgressEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PollEndedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Polls/poll-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PollEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PredictionBegin()
	{
		var text = File.ReadAllText("../../../EventSubData/Predictions/prediction-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PredictionBeginEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PredictionProgress()
	{
		var text = File.ReadAllText("../../../EventSubData/Predictions/prediction-progress.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PredictionProgressEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PredictionLockedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Predictions/prediction-locked.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PredictionLockedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void PredictionEndedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Predictions/prediction-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<PredictionEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void SuspiciousUserMessageEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/SuspiciousUser/suspicious-user-message.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<SuspiciousUserMessageEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void SuspiciousUserUpdatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/SuspiciousUser/suspicious-user-updated.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<SuspiciousUserUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChannelVipAdded()
	{
		var text = File.ReadAllText("../../../EventSubData/Vip/vip-added.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<VipEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChannelVipRemoved()
	{
		var text = File.ReadAllText("../../../EventSubData/Vip/vip-removed.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<VipEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void UserWarningAcknowledgedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Warning/warning-acknowledged.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<UserWarningAcknowledgedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void UserWarningSentEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Warning/warning-sent.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<UserWarningSentEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void GoalStart()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelGoal/channelgoal-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChannelGoalStartedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChannelGoalProgress()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelGoal/channelgoal-progress.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChannelGoalProgressEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ChannelGoalEnd()
	{
		var text = File.ReadAllText("../../../EventSubData/ChannelGoal/channelgoal-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ChannelGoalEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void HypeTrainStartedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/HypeTrain/hypetrain-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<HypeTrainStartedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void HypeTrainProgressedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/HypeTrain/hypetrain-progress.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<HypeTrainProgressedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void HypeTrainEndedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/HypeTrain/hypetrain-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<HypeTrainEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ShieldModeBeginEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ShieldMode/shieldmode-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ShieldModeBeginEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ShieldModeEndedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/ShieldMode/shieldmode-ended.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ShieldModeEndedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ShoutoutCreatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Shoutout/shoutout-sent.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ShoutoutCreatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void ShoutoutReceivedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/Shoutout/shoutout-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<ShoutoutReceivedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void UserUpdatedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/User/user-updated.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<UserUpdatedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CharityCampaignStartedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/CharityCampaign/charity-campaign-started.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CharityCampaignStartedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CharityCampaignProgressEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/CharityCampaign/charity-campaign-progress.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CharityCampaignProgressEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CharityDonationReceivedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/CharityCampaign/charity-donation-received.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CharityDonationReceivedEvent>>(text);

		Assert.NotNull(data.Payload);
	}

	[Fact]
	public void CharityCampaignStoppedEvent()
	{
		var text = File.ReadAllText("../../../EventSubData/CharityCampaign/charity-campaign-stopped.jsonl");
		var data = JsonSerializer.Deserialize<EventMessage<CharityCampaignStoppedEvent>>(text);

		Assert.NotNull(data.Payload);
	}
}