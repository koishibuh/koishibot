using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChatMessages.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
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

/*═══════════════════【 HANDLER 】═══════════════════*/
public record SendChatMessageHandler(
	IOptions<Settings> Settings,
	ITwitchIrcService StreamerClient,
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
	{
		return new SendChatMessageRequestBody
		{
			BroadcasterId = streamerId,
			SenderId = streamerId,
			Message = Message
		};
	}
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