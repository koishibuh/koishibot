using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.ChannelPoints;

public record ChannelPointsApi(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
ILogger<ChannelPointsApi> Log
) : IChannelPointsApi
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-custom-reward">Update Custom Reward Api Documentation</see>
	/// </summary>
	/// <returns></returns>
	public async Task UpdateCustomRewardStatus(string rewardTwitchId, bool status)
	{

		var parameters = new UpdateCustomRewardRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			RewardId = rewardTwitchId
		};

		var body = new UpdateCustomRewardRequestBody
		{
			IsEnabled = true,
			IsPaused = status
		};

		await TwitchApiRequest.UpdateCustomReward(parameters, body);
	}

	public async Task EnableRedemption(string rewardTwitchId) =>
		await UpdateCustomRewardStatus(rewardTwitchId, false);

	public async Task DisableRedemption(string rewardTwitchId) =>
		await UpdateCustomRewardStatus(rewardTwitchId, true);
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IChannelPointsApi
{
	Task UpdateCustomRewardStatus(string rewardTwitchId, bool status);
	Task EnableRedemption(string rewardTwitchId);
	Task DisableRedemption(string rewardTwitchId);
}