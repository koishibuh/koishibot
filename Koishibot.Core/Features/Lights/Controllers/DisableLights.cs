using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Lights.Extensions;
using Koishibot.Core.Features.Lights.Models;

namespace Koishibot.Core.Features.Lights.Controllers;

// == ⚫ DELETE == //

public class DisableLightsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpDelete("/api/led-lights")]
	public async Task<ActionResult> DisableLightsCommand()
	{
		await Mediator.Send(new DisableLightsCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record DisableLightsCommand() : IRequest;

// == ⚫ HANDLER == //

public record DisableLightsHandler(
	IAppCache Cache, ILightService LightService
	) : IRequestHandler<DisableLightsCommand>
{
	public async Task Handle(DisableLightsCommand command, CancellationToken cancel)
	{
		var lights = Cache.GetLights();
		lights ??= await LightService.ImportLights();

		var disableLights = new LightCommand().DisableLights();

		var dataCommandItems = lights
			.Select(x => lights.CreateDataCommand(disableLights, x.MacAddress))
			.ToArray();

		var payload = new Payload { dataCommandItems = dataCommandItems };

		var stringContent = Toolbox.CreateStringContent(payload);
		await LightService.SendBatchCommands(stringContent);

		lights.ForEach(x => x.Power = false);

		//Cache.RemoveLights();
	}
}