//using Koishibot.Core.Persistence.Cache.Enums;
//namespace Koishibot.Core.Features.Application.Controllers;

//// == ⚫ POST == //

//public class UpdateServiceStatusController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Service Status"])]
//	[HttpPatch("/api/service-status")]
//	public async Task<ActionResult> GetServiceStatus
//		([FromBody] UpdateServiceStatusCommand dto)
//	{
//		await Mediator.Send(dto);
//		return Ok();
//	}
//}

//// == ⚫ COMMAND == //

//public record UpdateServiceStatusCommand(
//	string ServiceName,
//	bool Status
//	) : IRequest;

//// == ⚫ HANDLER == //

//public record UpdateServiceStatusHandler(
//	IAppCache Cache
//	) : IRequestHandler<UpdateServiceStatusCommand>
//{
//	public async Task Handle
//		(UpdateServiceStatusCommand dto, CancellationToken cancel)
//	{
//		var name = Enum.Parse<ServiceName>(dto.ServiceName, true);
//		await Cache.UpdateServiceStatus(name, dto.Status);
//	}
//}