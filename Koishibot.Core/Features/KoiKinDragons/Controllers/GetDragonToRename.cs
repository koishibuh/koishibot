using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.KoiKinDragons.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.KoiKinDragons.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragon-quest")]
public class GetDragonToRenameController : ApiControllerBase
{
	[HttpGet("rename")]
	public async Task<ActionResult> GetDragonToRename()
	{
		var result = await Mediator.Send(new GetDragonToRenameQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetDragonToRenameHandler(
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