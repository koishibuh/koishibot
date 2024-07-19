namespace Koishibot.Core.Features.Obs.Interfaces;

public interface IObsService
{
	Task StartWebsocket();
	Task StopWebsocket();
	Task EnableStartStreamTimer();
	Task EnablePomodoroTimer();
	Task ChangeScene(string sceneName);
	Task EnableTimer();
};