using Koishibot.Core.Features.Lights.Extensions;
namespace Koishibot.Core.Features.Lights.Controllers;

// == ⚫ GET == //

public class ImportLightsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpGet("/api/led-lights/import")]
	public async Task<ActionResult> ImportLights()
	{
		await Mediator.Send(new ImportLightsCommand());
		return Ok();
	}
}

// == ⚫ QUERY == //

public record ImportLightsCommand(): IRequest;

// == ⚫ HANDLER == //

public record ImportLightsHandler(
	IAppCache Cache, ILightService LightService
) : IRequestHandler<ImportLightsCommand>
{
	public async Task Handle(ImportLightsCommand command, CancellationToken cancel)
	{
		var lights = await LightService.ImportLights();
		Cache.AddLights(lights);
	}
}