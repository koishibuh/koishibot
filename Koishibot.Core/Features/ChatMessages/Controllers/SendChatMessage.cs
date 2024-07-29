using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;

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

// == ⚫ HANDLER == //

public record SendChatMessageHandler(
	IOptions<Settings> Settings,
	ITwitchIrcService StreamerClient,
	ITwitchApiRequest TwitchApiRequest
	) : IRequestHandler<SendChatMessageCommand>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task Handle(SendChatMessageCommand command, CancellationToken cancel)
	{
		var body = command.CreateRequest(StreamerId);
		await TwitchApiRequest.SendChatMessage(body);
	}
}

// == ⚫ COMMAND == //

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


// == ⚫ VALIDATOR == //

public class SendChatMessageValidator
		: AbstractValidator<SendChatMessageCommand>
{
	public KoishibotDbContext Database { get; }

	public SendChatMessageValidator()
	{
		RuleFor(p => p.Message)
			.NotEmpty()
			.MaximumLength(500);
	}
}