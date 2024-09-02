using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest(
	ITwitchApiClient TwitchApiClient,
	ILogger<TwitchApiRequest> Log
	) : ITwitchApiRequest {}

public interface ITwitchApiRequest
{
	// ADS
	Task<StartAdResponse> StartAd(StartAdRequestBody requestBody);
	Task<AdScheduleData> GetAdSchedule(GetAdScheduleRequestParameters parameters);
	Task<SnoozeNextAdResponse> SnoozeNextAd(SnoozeNextAdRequestParameters parameters);

	// ANALYTICS
	Task GetGameAnalytics(GetGameAnalyticsRequestParameters parameters);

	// BITS
	Task<GetBitsLeaderboardData> GetBitsLeaderboard(GetBitsLeaderboardRequestParameters parameters);
	Task<GetCheermotesData> GetCheermotes(GetCheermotesRequestParameters parameters);

	// CHANNEL POINTS
	Task CreateCustomReward(CreateCustomRewardRequestParameters parameters, CreateCustomRewardRequestBody requestBody);
	Task DeleteCustomReward(DeleteCustomRewardRequestParameters parameters);
	Task<List<CustomRewardData>>GetCustomRewards(GetCustomRewardsParameters parameters);
	Task UpdateCustomReward(UpdateCustomRewardRequestParameters parameters, UpdateCustomRewardRequestBody requestBody);
	Task GetRewardRedemptions(GetRewardRedemptionsRequestParameters parameters);
	Task UpdateRedemptionStatus(UpdateRedemptionStatusRequestParameters parameters, UpdateRedemptionStatusRequestBody requestBody);

	// CHANNELS
	Task EditChannelInfo(EditChannelInfoRequestParameters parameters, EditChannelInfoRequestBody requestBody);
	Task GetChannelEditors(GetChannelEditorsRequestParameters parameters);
	Task GetChannelEmotes(GetChannelEmotesRequestParameters parameters);
	Task<List<ChannelInfoData>> GetChannelInfo(GetChannelInfoQueryParameters parameters);
	Task GetFollowedChannels(GetFollowedChannelsRequestParamaters parameters);
	Task GetFollowers(GetFollowersRequestParameters parameters);
	
	// CHARITY 
	Task GetCharityCampaign(GetCharityCampaignRequestParameters parameters);
	Task GetCharityCampaignDonations(GetCharityCampaignDonationsRequestParameters parameters);

	// CHAT
	Task GetChannelChatBadges(GetChannelChatBadgesParameters parameters);
	Task<ChatSettingsData> GetChatSettings(GetChatSettingsRequestParameters parameters);
	Task GetChatters(GetChattersRequestParameters parameters);
	Task GetEmoteSets(GetEmoteSetsRequestParameters parameters);
	Task GetGlobalChatBadges(GetChannelChatBadgesParameters parameters);
	Task GetGlobalEmotes();
	Task SendAnnouncement(SendAnnouncementRequestParameters parameters, SendAnnouncementRequestBody requestBody);
	Task SendChatMessage(SendChatMessageRequestBody requestBody);
	Task SendShoutout(SendShoutoutParameters parameters);
	Task UpdateChatSettings(UpdateChatSettingsRequestParamaters parameters, UpdateChatSettingsRequestBody requestBody);

	// CLIPS
	Task CreateClip(CreateClipRequestParameters parameters);
	Task<GetClipsResponse> GetClips(GetClipsRequestParameters parameters);

	// EVENTSUB
	Task CreateEventSubSubscription(List<CreateEventSubSubscriptionRequestBody> requestBody);
	//Task<CreateEventSubSubscriptionResponse> CreateEventSubSubscription(CreateEventSubSubscriptionRequestBody requestBody);

	// GAMES
	Task GetGames(GetGamesRequestParameters parameters);
	Task GetTopGames(GetTopGamesRequestParameters parameters);

	// GOALS
	Task GetCreatorGoals(GetCreatorGoalsRequestParameters parameters);

	// HYPE TRAIN
	Task GetHypeTrainEvents(GetHypeTrainEventsRequestParameters parameters);

	// MODERATION
	Task AddBlockedTerm(AddBlockedTermRequestParameters parameters, AddBlockedTermRequestBody requestBody);
	Task AddModerator(AddModeratorRequestParameter parameters);
	Task AddVip(AddVipRequestParameters parameters);
	Task BanUser(BanUserRequestParameters parameters, BanUserRequestBody requestBody);
	Task DeleteChatMessage(DeleteChatMessageParameters parameters);
	Task GetBannedUsers(GetBannedUsersRequestParameters parameters);
	Task GetBlockedTerms(GetBlockedTermsRequestParameters parameters);
	Task GetModerators(GetModeratorsRequestParameters parameters);
	Task GetShieldModeStatus(GetShieldModeStatusRequestParameters parameters);
	Task GetVips(GetVipsRequestParameters parameters);
	Task RemoveBlockedTerm(RemoveBlockedTermRequestParameters parameters);
	Task RemoveModerator(RemoveModeratorRequestParameters parameters);
	Task RemoveVip(RemoveVipRequestParameters parameters);
	Task UnbanUser(UnbanUserRequestParameters parameters);
	Task UpdateShieldModeStatus(UpdateShieldModeStatusRequestParameters parameters, UpdateShieldModeStatusRequestBody requestBody);
	Task WarnChatUser(WarnChatUserRequestParameters parameters, WarnChatUserRequestBody requestBody);

	// POLLS
	Task CreatePoll(CreatePollRequestBody requestBody);
	Task EndPoll(EndPollRequestBody requestBody);
	Task GetPolls(GetPollsRequestParameters parameters);

	// PREDICTIONS
	Task CreatePrediction(CreatePredictionRequestBody requestBody);
	Task EndPrediction(EndPredictionRequestBody requestBody);
	Task GetPredictions(GetPredictionsRequestParameters parameters);

	// RAIDS
	Task StartRaid(StartRaidRequestParameters parameters);
	Task CancelRaid(CancelRaidRequestParameters parameters);

	// SCHEDULE
	Task CreateStreamScheduleSegement(CreateStreamScheduleSegmentRequestParameters parameters, CreateStreamScheduleSegementRequestBody requestBody);
	Task DeleteStreamScheduleSegment(DeleteStreamScheduleSegmentRequestParameters parameters);
	Task<GetChannelStreamScheduleResponse> GetStreamSchedule(GetChannelStreamScheduleRequestParameters parameters);
	Task UpdateStreamSchedule(UpdateStreamScheduleRequestParameters parameters);
	Task UpdateStreamScheduleSegment(UpdateStreamScheduleSegmentRequestParameters parameters, UpdateStreamScheduleSegmentRequestBody requestBody);
	
	// SEARCH
	Task SearchCategories(SearchCategoriesRequestParameters parameters);
	Task SearchChannels(SearchChannelsRequestParameters parameters);

	// STREAMS
	Task CreateStreamMarker(CreateStreamMarkerRequestBody requestBody);
	Task<GetFollowedLiveStreamsResponse> GetFollowedLiveStreams(GetFollowedLiveStreamsRequestParameters parameters);
	Task<GetLiveStreamsResponse> GetLiveStreams(GetLiveStreamsRequestParameters parameters);
	Task GetStreamMarkers(GetStreamMarkersRequestParameters parameters);

	// SUBSCRIPTIONS
	Task CheckUserSubscription(CheckUserSubscriptionRequestParameters parameters);
	Task GetBroadcasterSubscriptions(GetBroadcasterSubscriptionsParameters parameters);

	// USERS
	Task BlockUser(BlockUserRequestParameters parameters);
	Task GetUserBlocklist(GetUserBlockListRequestParameters parameters);
	Task<List<UserData>> GetUsers(GetUsersRequestParameters parameters);
	Task UnblockUser(UnblockUserRequestParameters parameters);

	// VIDEOS
	Task DeleteVideos(DeleteVideosRequestParameters parameters);
	Task<GetVideosResponse> GetVideos(GetVideosRequestParameters parameters);

	// WHISPERS
	Task SendWhisper(SendWhisperRequestParameters parameters, SendWhisperRequestBody requestBody);
	}