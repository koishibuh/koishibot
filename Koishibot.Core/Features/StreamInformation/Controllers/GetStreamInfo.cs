using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.TwitchEventSub.Extensions;

namespace Koishibot.Core.Features.StreamInformation;

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
	IOptions<Settings> Settings, IAppCache Cache,
	IStreamInfoApi TwitchApi
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
				streamInfo = await TwitchApi.GetStreamInfo(StreamerId);
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

// == ⚫ TWITCH API == //

public partial record StreamInfoApi : IStreamInfoApi
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-information">Get Channel Info Documentation</see> <br/>
	/// BroadcasterId, BroadcasterLogin, BroadcasterName<br/>
	/// BroadcasterLanguage, GameId, GameName, GameTitle<br/>
	/// Delay, Tags, ContentClassificationLabels, IsBrandedContent<br/><br/>
	/// Works for online or offline stream
	/// </summary>
	/// <returns></returns>
	public async Task<StreamInfo> GetStreamInfo(string streamerId)
	{
		await TokenProcessor.EnsureValidToken();

		var result = await TwitchApi.Helix.Channels.GetChannelInformationAsync(streamerId);
		return result is null || result.Data.Length == 0
			? throw new Exception("Unable to get channel info from Api")
			: result.ConvertToDto();
	}
}