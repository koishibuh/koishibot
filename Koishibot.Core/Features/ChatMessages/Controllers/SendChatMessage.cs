using Koishibot.Core.Configurations;

namespace Koishibot.Core.Features.ChatMessages.Controllers;

// == ⚫ POST == //

public class SendChatMessageController : ApiControllerBase
{
	[HttpPost("/api/chat")]
	public async Task<ActionResult> SendChatMessage
		([FromBody] SendChatMessageCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record SendChatMessageCommand(
	string Message
	) : IRequest;

// == ⚫ HANDLER == //

public record SendChatMessageHandler(
	StreamerTwitchClient StreamerClient, ISignalrService Signalr
	) : IRequestHandler<SendChatMessageCommand>
{
	public async Task Handle(SendChatMessageCommand c, CancellationToken cancel)
	{
		await StreamerClient.SendMessageAsync("elysiagriffin", c.Message);
	}
}