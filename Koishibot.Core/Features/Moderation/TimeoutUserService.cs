using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Moderation;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record TimeoutUserService(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : ITimeoutUserService
{
	public async Task TwoSeconds(string userId)
	{
		// TimeoutUser
		var parameters = new BanUserRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			ModeratorId = Settings.Value.StreamerTokens.UserId
		};

		var body = new BanUserRequestBody
		{
			BanRequestData = new BanRequestData
			{
				UserId = userId,
				TimeoutDurationInSeconds = 2
			}
		};

		await TwitchApiRequest.BanUser(parameters, body);
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface ITimeoutUserService
{
	Task TwoSeconds(string userId);
}