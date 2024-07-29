using Koishibot.Core.Features.Obs.Interfaces;

namespace Koishibot.Core.Features.Obs;

public static class ObsActions
{
	public static async Task StartStream(this IObsService obs)
	{
		await obs.StartWebsocket();
		await obs.EnableStartStreamTimer();
	}

	public static async Task StartBreak(this IObsService obs)
	{
		await obs.ChangeScene("🌙 BRB");
		await obs.EnableTimer();
	}
}