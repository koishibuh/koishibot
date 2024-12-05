using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.OBS.Sources;
namespace Koishibot.Core.Features.Obs.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record InputListReceivedHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IRequestHandler<InputListReceivedCommand>
{
	public async Task Handle(InputListReceivedCommand receivedCommand, CancellationToken cancel)
	{
		var inputList = receivedCommand.CreateInputList();
		var obsItemVms = new List<ObsItemVm>();

		foreach (var item in inputList)
		{
			var result = await Database.ObsItems
				.FirstOrDefaultAsync(x => x.ObsId == item.ObsId);
			if (result is null)
			{
				Database.Add(item);
			}
			else if (result.ObsName != item.ObsName)
			{
				result.ObsName = item.ObsName;
				Database.Update(result);
			}

			var obsItem = result!.CreateVm();
			obsItemVms.Add(obsItem);
		}
		await Database.SaveChangesAsync(cancel);

		await Signalr.SendObsItems(obsItemVms);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record InputListReceivedCommand(
GetInputListResponse args
) : IRequest
{
	public List<ObsItem> CreateInputList() =>
		args.Inputs
			.Select(x => new ObsItem
			{
				ObsId = x.InputUuid,
				ObsName = x.InputName,
				Type = x.InputKind
			})
			.ToList();
}