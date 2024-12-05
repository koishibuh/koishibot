using Koishibot.Core.Persistence;
using Koishibot.Core.Services.OBS.Sources;
namespace Koishibot.Core.Features.Obs.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record InputKindListReceivedHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IRequestHandler<InputKindListReceivedCommand>
{
	public async Task Handle(InputKindListReceivedCommand receivedCommand, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: WIP
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record InputKindListReceivedCommand(
GetInputKindListResponse args
) : IRequest;