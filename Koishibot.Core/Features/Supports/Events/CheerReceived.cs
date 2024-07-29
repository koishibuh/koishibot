using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record CheerReceivedCommand(CheerReceivedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Channel Cheer EventSub Documentation</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record CheerReceivedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database,
	ILogger<CheerReceivedHandler> Log
	) : IRequestHandler<CheerReceivedCommand>
{
	public async Task Handle
		(CheerReceivedCommand command, CancellationToken cancel)
	{
		var userDto = new TwitchUserDto(
			command.args.CheererId,
			command.args.CheererLogin,
			command.args.CheererName);

		var user = await TwitchUserHub.Start(userDto);


		var cheer = new TwitchCheer().Initialize(user.Id, command.args.BitAmount, command.args.Message);
		await Database.AddCheer(cheer);

		var cheerVm = cheer.ConvertToVm(user.Name);
		await Signalr.SendStreamEvent(cheerVm);
	}
}