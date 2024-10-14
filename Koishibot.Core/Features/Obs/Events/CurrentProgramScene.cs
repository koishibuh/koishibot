using Koishibot.Core.Services.OBS.Scenes;
namespace Koishibot.Core.Features.Obs.Events;

// Gets current scene focus
/*═══════════════════【 HANDLER 】═══════════════════*/
public record CurrentProgramSceneHandler(
) : IRequestHandler<GetCurrentProgramSceneCommand>
{
	public async Task Handle(GetCurrentProgramSceneCommand request, CancellationToken cancel)
	{
		await Task.CompletedTask;
		//TODO: WIP
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record GetCurrentProgramSceneCommand(
GetCurrentProgramSceneResponse args
) : IRequest;