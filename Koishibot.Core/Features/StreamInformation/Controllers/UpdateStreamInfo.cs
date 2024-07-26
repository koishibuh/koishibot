using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation.Controllers;

// == ⚫ POST == //

public class UpdateStreamInfoController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Stream Info"])]
	[HttpPost("/api/stream-info/twitch")]
	public async Task<ActionResult> UpdateStreamInfo
		([FromBody] UpdateStreamInfoCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record UpdateStreamInfoCommand(
	string Title,
	string Category
	) : IRequest;


// == ⚫ HANDLER  == //

public record UpdateStreamInfoHandler(
	IOptions<Settings> Settings,
	IAppCache Cache,
			//IUpdateStreamInfoApi TwitchApi
			ITwitchApiRequest TwitchApiRequest
		//ITwitchApiClient TwitchApiService
	) : IRequestHandler<UpdateStreamInfoCommand>
{
	public async Task Handle
		(UpdateStreamInfoCommand dto, CancellationToken cancel)
	{
		// TODO: Update Category

		//await TwitchApi.UpdateStreamTitle(dto.Title);
		var parameters = new EditChannelInfoRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId
		};

		var body = new EditChannelInfoRequestBody
		{
			Title = "test4"
		};

		await TwitchApiRequest.EditChannelInfo(parameters, body);
	}
}

// == ⚫ TWITCH API  == //

//public record UpdateStreamInfoApi(
//	ITwitchAPI TwitchApi, IOptions<Settings> Settings,
//	IRefreshAccessTokenService TokenProcessor
//	) : IUpdateStreamInfoApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#modify-channel-information">Modify Channel Info Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task UpdateStreamTitle(string title)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var update = new ModifyChannelInformationRequest { Title = title };

//		await TwitchApi.Helix.Channels.ModifyChannelInformationAsync(StreamerId, update);
//	}
//}