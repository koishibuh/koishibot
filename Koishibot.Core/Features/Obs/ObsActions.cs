using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Scenes;

namespace Koishibot.Core.Features.Obs;

public static class ObsActions
{
	public static async Task StartStream(this IObsService obs)
	{
		//await obs.StartWebsocket();
		//await obs.EnableStartStreamTimer();
	}

	public static async Task StartBreak(this IObsService obs)
	{
		var request = new RequestWrapper<SetCurrentProgramSceneRequest>
		{
			RequestType = ObsRequests.SetCurrentProgramScene,
			RequestId = new Guid(),
			RequestData = new SetCurrentProgramSceneRequest
			{
				SceneUuid = "bc7908df-6e98-41ec-b79b-3378d198bb12"
			}
		};

		await obs.SendRequest(new ObsRequest<SetCurrentProgramSceneRequest>
		{
			Data = request
		});

		// Switch to break
		// Show Dandle game
	}


}