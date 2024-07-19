
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Lights.Extensions;
using Koishibot.Core.Features.Lights.Models;
namespace Koishibot.Core.Features.Lights.Controllers;


// == ⚫ POST == //

public class UpdateLightColorController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpPost("/api/led-lights/color")]
	public async Task<ActionResult> UpdateLightColorCommand()
	{
		await Mediator.Send(new UpdateLightColorCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record UpdateLightColorCommand() : IRequest;

// == ⚫ HANDLER == //

public record UpdateLightColorHandler(
	IAppCache Cache, ILightService LightService
	) : IRequestHandler<UpdateLightColorCommand>
{
	public async Task Handle(UpdateLightColorCommand command, CancellationToken cancel)
	{
		var lights = Cache.GetLights();
		lights ??= await LightService.ImportLights();

		var light = lights.Where(x => x.Name == "Striplight 1").First()
				?? throw new Exception("Light not found by name");

		light.UpdateRGB(255, 0, 153);
		var updateLight = new LightCommand().UpdateLightColor(light);

		var dataCommandItems = lights
			.Select(x => lights.CreateDataCommand(updateLight, x.MacAddress))
			.ToArray();

		var payload = new Payload { dataCommandItems = dataCommandItems };
		var stringContent = Toolbox.CreateStringContent(payload);
		await LightService.SendBatchCommands(stringContent);

		Cache.AddLights(lights);
	}
}