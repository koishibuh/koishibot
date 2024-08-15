using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.StreamInformation.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream")]
public class ReconnectStreamServices : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Stream"])]
	[HttpPost("reconnect")]
	public async Task<ActionResult> ReconnectStream()
	{
		await Mediator.Send(new ReconnectStreamCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ReconnectStreamHandler(
ILogger<ReconnectStreamHandler> Log,
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
IServiceScopeFactory ScopeFactory
) : IRequestHandler<ReconnectStreamCommand>
{
	public async Task Handle(ReconnectStreamCommand command, CancellationToken cancel)
	{
		if (await StreamIsOnline())
		{
			try
			{
				using var scope = ScopeFactory.CreateScope();
				var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

				await mediatr.Publish(new StreamReconnectCommand(), cancel);
			}
			catch (Exception ex)
			{
				Log.LogInformation($"{ex}");
			}
		}
		else
		{
			throw new Exception("Stream is offline");
		}
	}

	/*═════════◣  ◢═════════*/
	private async Task<bool> StreamIsOnline()
	{
		var parameters = new GetLiveStreamsRequestParameters
		{
			UserIds = new List<string> { Settings.Value.StreamerTokens.UserId },
			First = 1
		};

		var listStreamResponse = await TwitchApiRequest.GetLiveStreams(parameters);
		return listStreamResponse.Data?.Count == 1;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ReconnectStreamCommand : IRequest;