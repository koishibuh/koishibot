using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.RaidSuggestions;
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
	IRaidApi RaidApi, IChatMessageService BotIrc,
	IAppCache Cache, ISignalrService Signalr
	) : IRequestHandler<CancelRaidCommand>
{
	public async Task Handle
					(CancelRaidCommand command, CancellationToken cancel)
	{
		await RaidApi.CancelRaid();
		await BotIrc.RaidStatus(Code.RaidCancelled);

		var timer = new CurrentTimer();
		timer.ClearTimer();
		Cache.AddCurrentTimer(timer);

		var timerVm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(timerVm);
	}
}

// == ⚫ TWITCH API == //

public partial record RaidApi : IRaidApi
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#cancel-a-raid"/>Cancel A Raid</see>
	/// </summary>
	/// <returns></returns>
	public async Task CancelRaid()
	{
		await TokenProcessor.EnsureValidToken();

		await TwitchApi.Helix.Raids.CancelRaidAsync(StreamerId);
	}
}