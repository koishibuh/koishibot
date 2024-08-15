using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Api.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/live-streams")]
public class GetLiveStreamsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Live Streams"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetLiveStreams()
	{
		var result = await Mediator.Send(new GetLiveStreamsQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetLiveStreamsHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetLiveStreamsQuery, LiveStreamInfo>
{
	public async Task<LiveStreamInfo> Handle
	(GetLiveStreamsQuery query, CancellationToken cancel)
	{
		var streamerName = Settings.Value.StreamerTokens.UserLogin;

		var parameters = query.CreateParameters(streamerName);
		var result = await TwitchApiRequest.GetLiveStreams(parameters);
		if (result.Data is null || result.Data.Count == 0)
		{
			throw new Exception("Unable to get stream info");
		}

		var stream = result.Data[0];
		var liveStreamInfo = stream.ConvertToDto();
		return liveStreamInfo;
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetLiveStreamsQuery : IRequest<LiveStreamInfo>
{
	public GetLiveStreamsRequestParameters CreateParameters(string streamerName)
		=> new() { UserLogins = [ streamerName ] };
}