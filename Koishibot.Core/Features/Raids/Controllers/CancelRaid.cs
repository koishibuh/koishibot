using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Raids;

// == ⚫ DELETE == //

public class CancelRaidController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Outgoing Raid"])]
	[HttpDelete("/api/raid/twitch")]
	public async Task<ActionResult> CancelRaid()
	{
		await Mediator.Send(new CancelRaidCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record CancelRaidCommand() : IRequest;

// == ⚫ HANDLER == //

public record CancelRaidHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	ITwitchIrcService BotIrc,
	IAppCache Cache, ISignalrService Signalr
	) : IRequestHandler<CancelRaidCommand>
{
	public async Task Handle
					(CancelRaidCommand command, CancellationToken cancel)
	{
		var parameters = new CancelRaidRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId
		};


		await TwitchApiRequest.CancelRaid(parameters);

		//await BotIrc.RaidStatus(Code.RaidCancelled);

		var timer = new CurrentTimer();
		timer.ClearTimer();
		Cache.AddCurrentTimer(timer);

		var timerVm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(timerVm);
	}
}