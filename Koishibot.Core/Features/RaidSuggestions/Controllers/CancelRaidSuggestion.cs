using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
namespace Koishibot.Core.Features.RaidSuggestions.Controllers;

// == ⚫ DELETE == //

public class CancelRaidSuggestionController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Outgoing Raid"])]
	[HttpDelete("/api/raid/suggestions/")]
	public async Task<ActionResult> CancelRaidSuggestion()
	{
		await Mediator.Send(new CancelRaidSuggestionsCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record CancelRaidSuggestionsCommand() : IRequest;

// == ⚫ HANDLER == //

public record CancelRaidSuggestionHandler(
	IAppCache Cache, 
	IChatReplyService ChatReplyService,
	ISignalrService Signalr
	) : IRequestHandler<CancelRaidSuggestionsCommand>
{
	public async Task Handle
			(CancelRaidSuggestionsCommand c, CancellationToken cancel)
	{
		Cache.DisableRaidSuggestions()
					.ClearRaidSuggestions();

		// TODO: Store previous stream title/game to revert back to?

		await ChatReplyService.App(Command.RaidSuggestionsCancelled);

		var timer = new CurrentTimer();
		timer.ClearTimer();
		Cache.AddCurrentTimer(timer);

		var timerVm = timer.ConvertToVm();
		await Signalr.SendOverlayTimer(timerVm);
	}
}