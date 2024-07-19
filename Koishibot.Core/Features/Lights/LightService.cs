using Koishibot.Core.Features.Lights.Models;
using Newtonsoft.Json;

namespace Koishibot.Core.Features.Lights;

public interface ILightService
{
	Task<List<Light>> ImportLights();
	Task SendBatchCommands(StringContent content);
}

public record LightService(
	IOptions<Settings> Settings,
	IHttpClientFactory HttpClientFactory
	) : ILightService
{

	public async Task<List<Light>> ImportLights()
	{
		var httpClient = CreateGetClient();
		using var response = await httpClient.GetAsync("getMyBindDevicesAndState/MagicHue");

		if (response.IsSuccessStatusCode)
		{
			var responseBody = await response.Content.ReadAsStringAsync();

			var devices = JsonConvert.DeserializeObject<LightResponse>(responseBody);
			var lights = new List<Light>();

			foreach (var device in devices.Devices)
			{
				var light1 = new Light().SetInitialValues(device);
				lights.Add(light1);
			}

			return lights;
		}
		else
		{
			throw new Exception("Unable to import lights");
		}
	}

	public async Task SendBatchCommands(StringContent content)
	{
		var httpClient = CreateGetClient();
		using var response = await httpClient.PostAsync("sendCommandBatch/MagicHue", content);

		if (!response.IsSuccessStatusCode)
			throw new Exception("Unable to get devices");
	}

	public HttpClient CreateGetClient()
	{
		string ua = "MagicHue/1.9.3 (ANDROID,13,en-US)";
		var token = Settings.Value.MagicHueToken;

		var httpClient = HttpClientFactory.CreateClient("MagicHue");
		httpClient.DefaultRequestHeaders.Add("User-Agent", ua);
		httpClient.DefaultRequestHeaders.Add("token", token);
		return httpClient;
	}
}
