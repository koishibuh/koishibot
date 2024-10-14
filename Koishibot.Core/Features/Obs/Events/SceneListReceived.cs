using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.OBS.Scenes;
namespace Koishibot.Core.Features.Obs.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record SceneListReceivedHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IRequestHandler<SceneListReceivedCommand>
{
	public async Task Handle(SceneListReceivedCommand receivedCommand, CancellationToken cancel)
	{
		var scenes = receivedCommand.CreateObsItemList();

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
public record SceneListReceivedCommand(
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