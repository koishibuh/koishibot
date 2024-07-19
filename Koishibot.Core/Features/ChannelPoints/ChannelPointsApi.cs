using Koishibot.Core.Features.ChannelPoints.Interfaces;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;

namespace Koishibot.Core.Features.ChannelPoints;

public partial record ChannelPointsApi(IOptions<Settings> Settings,
	ITwitchAPI TwitchApi, IRefreshAccessTokenService TokenProcessor,
	ILogger<ChannelPointsApi> Log) : IChannelPointsApi
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;



	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-custom-reward">Update Custom Reward Api Documentation</see>
	/// </summary>
	/// <returns></returns>
	public async Task UpdateCustomRewardStatus(string rewardTwitchId, bool status)
	{
		await TokenProcessor.EnsureValidToken();

		var rewardRequest = new UpdateCustomRewardRequest
		{
			IsEnabled = true,
			IsPaused = status
		};

		var result = await TwitchApi.Helix.ChannelPoints.UpdateCustomRewardAsync(StreamerId, rewardTwitchId, rewardRequest);
		if (result is null)
			throw new Exception("Unable to update channel point reward status from Api");

		Log.LogInformation($"Pause Status for Channel Point Reward '{result.Data[0].Title}' has been set to {result.Data[0].IsPaused}");
	}

	public async Task EnableRedemption(string rewardTwitchId)
	{
		await UpdateCustomRewardStatus(rewardTwitchId, false);
	}

	public async Task DisableRedemption(string rewardTwitchId)
	{
		await UpdateCustomRewardStatus(rewardTwitchId, true);
	}
}