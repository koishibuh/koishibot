using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation.Controllers;

// == ⚫ GET == //

public class GetStreamInfoController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Stream Info"])]
	[HttpGet("/api/stream-info/twitch")]
	public async Task<ActionResult> GetStreamInfo()
	{
		var result = await Mediator.Send(new GetStreamInfoCommand());
		return Ok(result);
	}
}

// == ⚫ COMMAND == //

public record GetStreamInfoCommand() : IRequest<StreamInfoVm>;


// == ⚫ HANDLER == //

public record GetStreamInfoHandler(
	IAppCache Cache,
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
	) : IRequestHandler<GetStreamInfoCommand, StreamInfoVm>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task<StreamInfoVm> Handle
		(GetStreamInfoCommand command, CancellationToken cancel)
	{
		var streamInfo = Cache.GetStreamInfo();
		if (streamInfo is null)
		{
			if (Cache.GetStatusByServiceName(ServiceName.TwitchWebsocket))
			{
				var parameters = new GetChannelInfoQueryParameters
				{
					BroadcasterIds = new List<string> { Settings.Value.StreamerTokens.UserId }
				};


				var result = await TwitchApiRequest.GetChannelInfo(parameters);
					if (result.Count < 0) throw new Exception("User not found");

				streamInfo = new StreamInfo(
					new TwitchUserDto(
						result[0].BroadcasterId,
						result[0].BroadcasterLogin,
						result[0].BroadcasterName),
						result[0].StreamTitle,
						result[0].CategoryName,
						result[0].CategoryId);

				Cache.UpdateStreamInfo(streamInfo);

				return streamInfo.ConvertToVm();
			}
			else
			{
				return new StreamInfoVm("", "", "");
			}
		}

		return streamInfo.ConvertToVm();
	}
}