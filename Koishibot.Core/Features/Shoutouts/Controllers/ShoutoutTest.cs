using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Shoutouts.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/shoutout")]
public class ShoutoutTestController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Shoutout"])]
	[HttpPost("test")]
	public async Task<ActionResult> ShoutoutTest()
	{
		await Mediator.Send(new ShoutoutTestCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ShoutoutTestHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
ISignalrService SignalrService
) : IRequestHandler<CreateShoutoutCommand>
{
	public async Task Handle(CreateShoutoutCommand command, CancellationToken cancel)
	{
		var vod = "https://player.twitch.tv/?muted=false&parent=twitch.tv&video=2263568200";

		await SignalrService.SendRaidShoutout(vod);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ShoutoutTestCommand : IRequest;