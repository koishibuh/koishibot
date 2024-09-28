using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.ChannelPoints.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest")]
public class GetDragonToRenameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dragon Quest"])]
	[HttpGet("rename")]
	public async Task<ActionResult> GetDragonToRename()
	{
		var result = await Mediator.Send(new GetDragonToRenameQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetDragonToRenameHandler(
// IOptions<Settings> Settings,
// ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database
) : IRequestHandler<GetDragonToRenameQuery, KoiKinDragonVm>
{
	public async Task<KoiKinDragonVm> Handle
		(GetDragonToRenameQuery query, CancellationToken cancel)
	{
		var dragonVm = await Database.GetLastUnnamedAdultDragon();
		if (dragonVm is null)
			throw new CustomException("No unnamed adult dragons found");

		return dragonVm;
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetDragonToRenameQuery : IRequest<KoiKinDragonVm>;