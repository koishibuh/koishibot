using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.RaidSuggestions;

// == ⚫ POST == //

public class OpenRaidSuggestionsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Outgoing Raid"])]
	[HttpPost("/api/raid/suggestions/")]
	public async Task<ActionResult> OpenRaidSuggestionsCommand()
	{
		await Mediator.Send(new OpenRaidSuggestionsCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record OpenRaidSuggestionsCommand() : IRequest;

// == ⚫ HANDLER == //

public record OpenRaidSuggestionsHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	ITwitchIrcService BotIrc,
	IAppCache Cache, 
	//IRaidSuggestionsApi TwitchApi,
	ISelectRaidCandidatesService SelectRaidCandidates, ISignalrService Signalr
	) : IRequestHandler<OpenRaidSuggestionsCommand>
{
	public async Task Handle
			(OpenRaidSuggestionsCommand command, CancellationToken cancel)
	{
		Cache.EnableRaidSuggestions();

		await BotIrc.PostRaidSuggestionsOpen();

		//await TwitchApi.UpdateStreamTitle("Ending Soon! Searching for Raid Target");

		//await new CurrentTimer()
		//					.SetSuggestionPoll()
		//					.AddToCache(Cache)
		//					.ConvertToVm()
		//					.UpdateOverlay(Signalr);

		// Set on Vue Client as well

		// Todo: Show current live followed streams 

		await Task.Delay(new TimeSpan(0, 2, 00));

		await SelectRaidCandidates.Start();
	}
}