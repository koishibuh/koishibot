namespace Koishibot.Core.Features.StreamInformation.ViewModels;

public record StreamInfoVm(
		string StreamTitle,
		string Category,
		string CategoryId)
{
	public async Task SendToVue(ISignalrService signalr)
	{
		await signalr.SendStreamInfo(this);
	}
}