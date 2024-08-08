using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Lights.Extensions;
using Koishibot.Core.Features.Lights.Models;
namespace Koishibot.Core.Features.Lights.Controllers;

// == ⚫ POST == //

public class EnableLightsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpPost("/api/led-lights")]
	public async Task<ActionResult> EnableLights([FromBody] EnableLightsCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record EnableLightsCommand(
	string LightName) : IRequest;

// == ⚫ HANDLER == //

public record EnableLightsHandler(
	IAppCache Cache, ILightService LightService
	) : IRequestHandler<EnableLightsCommand>
{

	public async Task Handle(EnableLightsCommand command, CancellationToken cancel)
	{
		var lights = Cache.GetLights();
		lights ??= await LightService.ImportLights();

		var enableLights = new LightCommand().EnableLights();

		var dataCommandItems = lights
			.Select(x => lights.CreateDataCommand(enableLights, x.MacAddress))
			.ToArray();

		var payload = new Payload { dataCommandItems = dataCommandItems };

		var stringContent = Toolbox.CreateStringContent(payload);
		await LightService.SendBatchCommands(stringContent);

		Cache.AddLights(lights);
	}
}