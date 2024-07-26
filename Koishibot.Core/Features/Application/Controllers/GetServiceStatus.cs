//using Koishibot.Core.Features.Application.Models;
//using Koishibot.Core.Persistence.Cache.Enums;
//namespace Koishibot.Core.Features.Application.Controllers;

//// == ⚫ GET == //

//public class GetServiceStatusController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Service Status"])]
//	[HttpGet("/api/service-status")]
//	public async Task<ActionResult> GetServiceStatus()
//	{
//		var result = await Mediator.Send(new GetServiceStatusCommand());
//		return Ok(result);
//	}
//}

//// == ⚫ QUERY == //

//public record GetServiceStatusCommand()
//	: IRequest<List<ServiceStatusVm>>;

//// == ⚫ HANDLER == //

//public record GetServiceStatusHandler(
//	IAppCache Cache
//	) : IRequestHandler<GetServiceStatusCommand, List<ServiceStatusVm>>
//{
//	public async Task<List<ServiceStatusVm>> Handle
//		(GetServiceStatusCommand command, CancellationToken cancel)
//	{
//		ServiceStatus result = Cache.Get<ServiceStatus>(CacheName.ServiceStatus);

//		var list = typeof(ServiceStatus)
//			.GetProperties()
//			.Select(x => new ServiceStatusVm(x.Name, (bool)x.GetValue(result)!))
//			.ToList();

//		return await Task.FromResult(list);
//	}
//}