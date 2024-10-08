namespace Koishibot.Core.Services.OBS.Scenes;

// Gets current scene focus
/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetCurrentProgramSceneHandler() : IRequestHandler<GetCurrentProgramSceneCommand>
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

/*══════════════════【 RESPONSE 】══════════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getcurrentprogramscene">Obs Documentation</see>
/// </summary>
public class GetCurrentProgramSceneResponse
{
	/// <summary>
	/// Scene name to set as the current program scene
	/// </summary>
	public string SceneName { get; set; } = string.Empty;

	/// <summary>
	/// Scene UUID to set as the current program scene
	/// </summary>
	public string SceneUuid { get; set; } = string.Empty;
}