using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.StreamInformation.ViewModels;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
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
		var result = await Mediator.Send(new GetStreamInfoQuery());
		return Ok(result);
	}
}

// == ⚫ HANDLER == //

public record GetStreamInfoHandler(
	IAppCache Cache,
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	KoishibotDbContext Database
	) : IRequestHandler<GetStreamInfoQuery, StreamInfoVm>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task<StreamInfoVm> Handle
		(GetStreamInfoQuery query, CancellationToken cancel)
	{
		if (Settings.Value.StreamerTokens.RefreshToken is null)
		{
			throw new Exception("Refresh Token not valid");
		}

		var streamInfo = Cache.GetStreamInfo();
		if (streamInfo is null)
		{
			if (Cache.GetStatusByServiceName(ServiceName.TwitchWebsocket))
			{
				var parameters = query.CreateParameters(StreamerId);
				var result = await TwitchApiRequest.GetChannelInfo(parameters);

				if (result.Count < 0) throw new Exception("User not found");

				// Add Category name and Id to Database
				var category = new StreamCategory
				{
					TwitchId = result[0].CategoryId,
					Name = result[0].CategoryName
				};

				await category.UpsertEntry(Database);

				//await Database.UpdateEntry(category);

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


// == ⚫ QUERY == //

public record GetStreamInfoQuery(
	) : IRequest<StreamInfoVm>
{
	public GetChannelInfoQueryParameters CreateParameters(string streamerId)
		=> new GetChannelInfoQueryParameters { BroadcasterIds = [streamerId] };
};