using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.KoiKinDragons.Controllers;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dragons")]
public class GetDragonListController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult> GetDragonListPage()
	{
		var result = await Mediator.Send(new GetDragonListQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetDragonListHandler(
IAppCache Cache,
KoishibotDbContext Database
) : IRequestHandler<GetDragonListQuery, List<KoiKinDragonSiteVm>>
{
	public async Task<List<KoiKinDragonSiteVm>> Handle(GetDragonListQuery query, CancellationToken cancel)
	{
		var result = await Database.KoiKinDragons
			.Include(x => x.TwitchUser)
			.Select(x => new KoiKinDragonSiteVm(x.Id, x.Name, x.Code, x.TwitchUser.Name))
			.ToListAsync(cancel);

		return result;
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetDragonListQuery : IRequest<List<KoiKinDragonSiteVm>>;

public record KoiKinDragonSiteVm(int Id, string Name, string Code, string Username);