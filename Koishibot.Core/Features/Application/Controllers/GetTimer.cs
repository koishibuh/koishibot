//using Koishibot.Core.Features.Common.Models;
//namespace Koishibot.Core.Features.Application.Controllers;

//// == ⚫ GET == //

//public class GetTimerController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Timer"])]
//	[HttpGet("/api/timer")]
//	public async Task<ActionResult> GetTimer()
//	{
//		var result = await Mediator.Send(new GetTimerCommand());
//		return Ok(result);
//	}
//}

//// == ⚫ QUERY == //

//public record GetTimerCommand()
//	: IRequest<OverlayTimerVm>;

//// == ⚫ HANDLER == //

//public record GetTimerHandler(
//	IAppCache Cache
//	) : IRequestHandler<GetTimerCommand, OverlayTimerVm>
//{
//	public async Task<OverlayTimerVm> Handle
//		(GetTimerCommand command, CancellationToken cancel)
//	{
//		var result = Cache.GetCurrentTimer();
//		return await Task.FromResult(result.ConvertToVm());
//	}
//}