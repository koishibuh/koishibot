using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
namespace Koishibot.Core.Features.Polls;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/poll")]
public class PollTestController : ApiControllerBase
{
	[HttpPost("test")]
	public async Task<ActionResult> PollTest()
	{
		await Mediator.Send(new PollTestCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record PollTestHandler(
ISignalrService Signalr
) : IRequestHandler<PollTestCommand>
{
	public async Task Handle
		(PollTestCommand c, CancellationToken cancel)
	{
		var pollStart = new PollVm(
			"1",
			"What should we name the dragon?",
			DateTimeOffset.UtcNow,
			DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5),
			// TimeSpan.FromMinutes(1),
			// TimeSpan.FromMinutes(5),
			TimeSpan.FromSeconds(42),
			new List<PollChoiceInfo> { new("This is a test choice", 00), new("This is another choice", 00), new("Choice choice", 00) }
			);

		await Signalr.SendPollStarted(pollStart);

		await Task.Delay(TimeSpan.FromSeconds(5));

		var pollVote1 = new List<PollChoiceInfo> { new("This is a test choice", 1), new("This is another choice", 0), new("Choice choice", 0) };
		await Signalr.SendPollVote(pollVote1);

		await Task.Delay(TimeSpan.FromSeconds(5));

		var pollVote2 = new List<PollChoiceInfo> { new("This is a test choice", 1), new("This is another choice", 1), new("Choice choice", 0) };
		await Signalr.SendPollVote(pollVote2);

		await Task.Delay(TimeSpan.FromSeconds(5));

		var pollVote3 = new List<PollChoiceInfo> { new("This is a test choice", 1), new("This is another choice", 2), new("Choice choice", 0) };
		await Signalr.SendPollVote(pollVote3);

		await Task.Delay(TimeSpan.FromSeconds(6));

		await Signalr.SendPollEnded("Choice2");
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record PollTestCommand : IRequest;