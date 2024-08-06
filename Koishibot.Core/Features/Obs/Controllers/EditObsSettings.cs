namespace Koishibot.Core.Features.Obs.Controllers;

// == ⚫ POST == //

public class EditObsSettingsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("/api/obs")]
	public async Task<ActionResult> EditObsSettings
		([FromBody] EditObsSettingsCommand c)
	{
		await Mediator.Send(c);
		return Ok();
	}
}

// == ⚫ HANDLER == //

public record EditObsSettingsHandler(
	IOptions<Settings> Settings
	) : IRequestHandler<EditObsSettingsCommand>
{
	public async Task Handle
			(EditObsSettingsCommand c, CancellationToken cancel)
	{
		//Settings.Value.ObsSettings.WebsocketUrl = c.Request.WebsocketUrl;
		//Settings.Value.ObsSettings.Port = c.Request.Port;
		//Settings.Value.ObsSettings.Password = c.Request.Password;		
		Settings.Value.ObsSettings.WebsocketUrl = c.WebsocketUrl;
		Settings.Value.ObsSettings.Port = c.Port;
		Settings.Value.ObsSettings.Password = c.Password;

		await Task.CompletedTask;
	}
}

// == ⚫ COMMAND == //

public record EditObsSettingsCommand(
		string WebsocketUrl,
	string Port,
	string Password
	) : IRequest;

//public record ObsRequest(
//	string WebsocketUrl,
//	string Port,
//	string Password
//	);