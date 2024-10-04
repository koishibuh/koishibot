using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Services.OBS.Scenes;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetSceneListHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IRequestHandler<GetSceneListCommand>
{
	public async Task Handle(GetSceneListCommand command, CancellationToken cancel)
	{
		var scenes = command.CreateObsItemList();

		foreach (var scene in scenes)
		{
			var sceneDb = await Database.FindObsItemByObsId(scene.ObsId);
			if (sceneDb.NotInDatabase())
			{
				await Database.UpdateEntry(scene);
			}
			else
			{
				if (sceneDb!.NameNotUpdated(scene)) continue;

				sceneDb.ObsName = scene.ObsName;
				await Database.UpdateEntry(sceneDb);
			}
		}

		await Signalr.SendInfo(scenes[0].ObsName);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record GetSceneListCommand(
GetSceneListResponse args
) : IRequest
{
	public List<ObsItem> CreateObsItemList() =>
		args.Scenes.Select(x => new ObsItem
		{
			ObsId = x.SceneUuid ?? string.Empty,
			ObsName = x.SceneName ?? string.Empty,
			Type = ObsItemType.Scene
		}).ToList();
}

/*══════════════════【 RESPONSE 】══════════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getscenelist">Obs Documentation</see>
/// </summary>
public class GetSceneListResponse
{
	/// <summary>
	/// Current program scene name.Can be null if internal state desync
	/// </summary>
	public string? CurrentProgramSceneName { get; set; }

	/// <summary>
	/// Current program scene UUID.Can be null if internal state desync
	/// </summary>
	public string? CurrentProgramSceneUuid { get; set; }

	/// <summary>
	///  Current preview scene name. null if not in studio mode
	/// </summary>
	public string? CurrentPreviewSceneName { get; set; }

	/// <summary>
	/// Current preview scene UUID. null if not in studio mode
	/// </summary>
	public string? CurrentPreviewSceneUuid { get; set; }

	/// <summary>
	/// 	Array of scenes
	/// </summary>
	public List<Scene> Scenes { get; set; }
}

public class Scene
{
	/// <summary>
	/// Gets the scene index.
	/// </summary>
	public int SceneIndex { get; set; }

	/// <summary>
	/// Gets the scene name.
	/// </summary>
	public string? SceneName { get; set; }

	/// <summary>
	/// UUID of the scene.
	/// </summary>
	public string? SceneUuid { get; set; }
}