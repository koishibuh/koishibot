using Koishibot.Core.Features.Lights.Extensions;
using Koishibot.Core.Features.Lights.Models;
namespace Koishibot.Core.Features.Lights.Controllers;

// == ⚫ GET == //

public class ImportLightsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["LedLights"])]
	[HttpGet("/api/led-lights/import")]
	public async Task<ActionResult> ImportLights()
	{
		var result = await Mediator.Send(new ImportLightsCommand());
		return Ok(result);
	}
}

// == ⚫ QUERY == //

public record ImportLightsCommand(): IRequest<List<LightVm>>;

// == ⚫ HANDLER == //

public record ImportLightsHandler(
	IAppCache Cache, ILightService LightService
) : IRequestHandler<ImportLightsCommand, List<LightVm>>
{
	public async Task<List<LightVm>> Handle(ImportLightsCommand command, CancellationToken cancel)
	{
		var lights = await LightService.ImportLights();
		Cache.AddLights(lights);

		return lights.Select(x => new LightVm(x.Name, x.Power, x.GetHexCode(), false)).ToList();
	}
}