using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Persistence.Cache.Enums;
using OBSStudioClient;
using OBSStudioClient.Enums;
using System.ComponentModel;

namespace Koishibot.Core.Features.Obs;

public record ObsService
	(IOptions<Settings> Settings, IAppCache Cache,
	ObsClient ObsClient, ILogger<ObsService> Log) : IObsService
{
	public ObsSettings ObsSettings => Settings.Value.ObsSettings;
	public bool StreamConnected = false;
	public string LastScene = "🌙 BRB";


	public async Task StartWebsocket()
	{
		try
		{
			ObsClient.ConnectionClosed += OnConnectionClosed;
			ObsClient.PropertyChanged += OnPropertyChanged;
			await ObsClient.ConnectAsync(false, ObsSettings.Password, ObsSettings.WebsocketUrl);
		}
		catch (Exception ex)
		{
			Log.LogError("There was an issue starting ObsWebsocket: {ex}", ex); 
		}
	}

	public async Task StopWebsocket()
	{
		ObsClient.Disconnect(); 
		ObsClient.Dispose(); 
		
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, false);
		Log.LogInformation($"Obs Websocket closed.");
	}

	public async Task ChangeScene(string sceneName)
	{
		try
		{
			await ObsClient.SetCurrentProgramScene(sceneName);
			Log.LogInformation($"Obs changing scenes.");
		}
		catch
		{
			Log.LogError("Unable to switch OBS scenes");
		}
	}

	public async Task ChangeBrowserSource(string sourceName, string url)
	{
		// https://github.com/obs-websocket-community-projects/obs-websocket-js/discussions/319
		// Todo: Needs testing
		await ObsClient.SetInputSettings("nameofsource", new Dictionary<string, object> { { "url", "theurl" } });
		Log.LogInformation($"Obs updated browser source.");
	}

	public async Task SwitchToLastScene()
	{
		await ObsClient.SetCurrentProgramScene(LastScene);
		Log.LogInformation($"Obs switching to last scene.");
	}

	public async Task EnableTimer()
	{
		await ObsClient.SetSourceFilterEnabled("BRB Timer", "Start", true);
		Log.LogInformation($"Enabled OBS timer");
	}

	public async Task EnableStartStreamTimer()
	{
		await ObsClient.SetSourceFilterEnabled("Start Stream Timer", "Start", true);
		Log.LogInformation($"Enabled Start Stream timer");
	}

	public async Task EnablePomodoroTimer()
	{
		await ObsClient.SetSourceFilterEnabled("1 Hour Timer", "Start", true);
		Log.LogInformation($"Enabled Pomodoro OBS timer");
	}

	public async Task Test()
	{
		LastScene = await ObsClient.GetCurrentProgramScene();
		await ObsClient.SetCurrentProgramScene("🌙 BRB");
		await Task.Delay(6000);
		await ObsClient.SetCurrentProgramScene(LastScene);
	}

	public async void OnConnectionClosed(object? sender, ConnectionClosedEventArgs e)
	{
		// Reconnect if there is an error? Not an error if Stream is finished 
		//await ServiceStatusCache.Update(new ServiceStatusDTO(ServiceName.ObsWebsocket, false));
		await Task.CompletedTask;
	}

	public async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		var result = sender as ObsClient 
			?? throw new Exception("ObsClient OnPropertyChanged Bork");

		if (result.ConnectionState == ConnectionState.Connecting)
		{
			// Do a thing				
			return;
		}
		
		if (result.ConnectionState == ConnectionState.Connected)
		{
			StreamConnected = true;
			await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, true);
			return;
		}

		if (result.ConnectionState == ConnectionState.Disconnected)
		{
			await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, false);
			return;
		}
	}
}
