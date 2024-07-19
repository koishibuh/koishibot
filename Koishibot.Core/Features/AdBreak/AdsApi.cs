using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.Common;
namespace Koishibot.Core.Features.AdBreak;


public record AdsApi(ILogger<AdsApi> Log,
		ITwitchAPI TwitchApi, IOptions<Settings> Settings,
		IRefreshAccessTokenService TokenProcessor) : IAdsApi
{
	public string UserId => Settings.Value.StreamerTokens.UserId;

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#snooze-next-ad">Snooze Next Ad</see>
	/// </summary>
	/// <returns></returns>
	//public async Task SnoozeNextAd()
	//{

	//}

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-ad-schedule">Get Ad Schedule</see><br/>
	/// When stream is offline, this will return what would've been the next ad time when stream was last live and LastAdAt set to 0<br/>
	/// Ex: Duration 180, LastAdAt "0", NextAdAt "1716336764", PrerollFreeTime 0, SnoozeCount 3, SnoozeRefreshAt 0
	/// </summary>
	/// <returns></returns>
	public async Task<AdBreakInfo> GetAdSchedule()
	{
		await TokenProcessor.EnsureValidToken();

		var result = await TwitchApi.Helix.Channels.GetAdScheduleAsync(UserId);
		if (result.Data.Length == 0)
		{ throw new Exception("Ad schedule was not found in API"); }

		var nextAdStillValid = Toolbox.NextAdTimeStillValid(result.Data[0].NextAdAt);

		var retryCount = 0;
		while (nextAdStillValid is false && retryCount < 3)
		{
			var delay = TimeSpan.FromSeconds(Math.Pow(4, retryCount));
			await Task.Delay(delay);
			retryCount++;

			Log.LogInformation($"{retryCount} Attempt to get latest ad schedule");
			result = await TwitchApi.Helix.Channels.GetAdScheduleAsync(UserId);
			nextAdStillValid = Toolbox.NextAdTimeStillValid(result.Data[0].NextAdAt);
		}

		if (nextAdStillValid)
		{
			Log.LogInformation($"Latest ad schedule valid");
			Log.LogInformation($"Last ad at: {DateTimeOffset.FromUnixTimeSeconds(long.Parse(result.Data[0].LastAdAt))}, Next ad at: {DateTimeOffset.FromUnixTimeSeconds(long.Parse(result.Data[0].NextAdAt))}");
			return new AdBreakInfo().Set(result);
		}
		else
		{
			Log.LogError("Unable to get latest ad schedule from API");
			Log.LogInformation($"Last ad at: {DateTimeOffset.FromUnixTimeSeconds(long.Parse(result.Data[0].LastAdAt))}, Next ad at: {DateTimeOffset.FromUnixTimeSeconds(long.Parse(result.Data[0].NextAdAt))}");
			return new AdBreakInfo().Set();
		}
	}
}