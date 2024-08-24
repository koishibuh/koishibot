using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChatMessages.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/chat")]
public class SendChatMessageController : ApiControllerBase
{
	[HttpPost]
	public async Task<ActionResult> SendChatMessage
	([FromBody] SendChatMessageCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record SendChatMessageHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<SendChatMessageCommand>
{
	public async Task Handle(SendChatMessageCommand command, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		var body = command.CreateRequest(streamerId);
		await TwitchApiRequest.SendChatMessage(body);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record SendChatMessageCommand(
string Message
) : IRequest
{
	public SendChatMessageRequestBody CreateRequest(string streamerId)
		=> new() {
		BroadcasterId = streamerId,
		SenderId = streamerId,
		Message = Message
		};
}

/*══════════════════【 VALIDATOR 】══════════════════*/
public class SendChatMessageValidator
: AbstractValidator<SendChatMessageCommand>
{
	public SendChatMessageValidator()
	{
		RuleFor(p => p.Message)
		.NotEmpty()
		.MaximumLength(500);
	}
}