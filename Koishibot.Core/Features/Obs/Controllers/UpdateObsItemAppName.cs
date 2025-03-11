using Koishibot.Core.Persistence;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Enums;
using Koishibot.Core.Services.OBS.Sources;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs/item")]
public class UpdateObsItemAppNameController : ApiControllerBase
{
	[HttpPatch]
	public async Task<ActionResult> UpdateObsItemAppName
		([FromQuery] int id, [FromBody] UpdateObsItemAppDto dto)
	{
		var command = new UpdateObsItemAppNameCommand(id, dto.AppName);

		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateObsItemAppNameHandler(
KoishibotDbContext Database,
IObsService ObsService
) : IRequestHandler<UpdateObsItemAppNameCommand>
{
	public async Task Handle
		(UpdateObsItemAppNameCommand c, CancellationToken cancel)
	{
		var obsItemDb = await Database.ObsItems
			.FirstOrDefaultAsync(x => x.Id == c.Id);

		if (obsItemDb is null)
			throw new Exception($"ObsItem with {c.Id} not found");

		obsItemDb.AppName = c.AppName;

		if (obsItemDb.ObsName.Contains("🤖") is false)
		{
			obsItemDb.ObsName = $"{obsItemDb.ObsName} 🤖";
			
			var data = new RequestWrapper<SetInputNameRequest>
			{
				RequestType = ObsRequests.SetInputName,
				RequestId = new Guid(),
				RequestData = new SetInputNameRequest
				{
					InputUuid = obsItemDb.ObsId,
					NewInputName = obsItemDb.ObsName
				}
			};

			var request = new ObsRequest<SetInputNameRequest>
			{
				Op = OpCodeType.Request,
				Data = data
			};

			await ObsService.SendRequest(request);
		}

		Database.Update(obsItemDb);
		await Database.SaveChangesAsync(cancel);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateObsItemAppNameCommand(
int Id,
string AppName
) : IRequest
{
	public async Task<bool> IsAppNameUnique(KoishibotDbContext database)
	{
		var result = await database.ObsItems
			.FirstOrDefaultAsync(p => p.AppName == AppName);

		return result is null;
	}
}

public record UpdateObsItemAppDto(string AppName);

/*══════════════════【 VALIDATOR 】══════════════════*/
public class CreateRewardValidator
	: AbstractValidator<UpdateObsItemAppNameCommand>
{
	private KoishibotDbContext Database { get; }

	public CreateRewardValidator(KoishibotDbContext context)
	{
		Database = context;

		// RuleFor(p => p.Id)
		// 	.NotEmpty();
		//
		// RuleFor(p => p.AppName)
		// 	.NotEmpty();

		RuleFor(p => p)
			.MustAsync(IsAppNameUnique)
			.WithMessage("App name already exists");
	}

	private async Task<bool> IsAppNameUnique
		(UpdateObsItemAppNameCommand command, CancellationToken cancel)
		=> await command.IsAppNameUnique(Database);
}