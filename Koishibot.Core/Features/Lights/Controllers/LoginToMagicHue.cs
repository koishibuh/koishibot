using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Lights.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
namespace Koishibot.Core.Features.Lights.Controllers;

// == ⚫ POST == //

public class LoginToMagicHueController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpPost("/api/led-lights/login")]
	[AllowAnonymous]
	public async Task<ActionResult> LoginToMagicHue
		([FromBody] LoginToMagicHueCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record LoginToMagicHueCommand(
	string Email,
	string Password
	) : IRequest;

// == ⚫ HANDLER == //

public record LoginToMagicHueHandler(
	IOptions<Settings> Settings, 
	IAppCache Cache,
	IHttpClientFactory HttpClientFactory
	) : IRequestHandler<LoginToMagicHueCommand>
{
	public async Task Handle(LoginToMagicHueCommand command, CancellationToken cancel)
	{
		var payload = CreateMagicHomeLoginPayload(command);
		var content = Toolbox.CreateStringContent(payload);

		var httpClient = HttpClientFactory.CreateClient("MagicHue");
		var response = await httpClient.PostAsync("login/MagicHue", content);

		if (response.IsSuccessStatusCode)
		{
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<LoginResponse>(responseBody)
				?? throw new Exception("Login Response was null");

			if (result.Code == "10033")
			{
				throw new Exception("Login invalid");
			}

			Settings.Value.MagicHueToken = result.Token;
		}
		else
		{
			throw new Exception("Unable to login to Magic");
		}
	}

	public MagicHomeLogin CreateMagicHomeLoginPayload(LoginToMagicHueCommand command)
	{
		var clientId = Toolbox.CreateRandom32CharString();
		var hashedPassword = Toolbox.CreateMD5Hash(command.Password);

		return new MagicHomeLogin
		{
			userID = command.Email,
			password = hashedPassword,
			clientID = clientId
		};
	}
}