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
		
		var storedObsItemIds = await Database.ObsItems.Select(x => x.Id).ToListAsync();


		foreach (var item in inputList)
		{
			var storedEntry = await Database.ObsItems
				.FirstOrDefaultAsync(x => x.ObsId == item.ObsId);
			if (storedEntry is null)
			{
				Database.Add(item);
				return;
			}
			
			 if (storedEntry.ObsName != item.ObsName)
			{
				storedEntry.ObsName = item.ObsName;
				Database.Update(storedEntry);
			}
			
			if (storedEntry.AppName is not null && item.ObsName.Contains("🤖") is false)
			{
				storedEntry.ObsName = $"{storedEntry.ObsName} 🤖";
				Database.Update(storedEntry);
			}

			storedObsItemIds.Remove(storedEntry.Id);

			var obsItem = storedEntry!.CreateVm();
			obsItemVms.Add(obsItem);
		}
		
		// Check if the list has anything remaining
		if (storedObsItemIds.Count != 0)
		{
			foreach (var obsItemId in storedObsItemIds)
			{
				// Check if Obj has a App Name
				var result = await Database.FindObsItemById(obsItemId);

				if (result is null)
				{
					// Something bork?
					return;
				}

				if (result.AppName is not null)
				{
					// Need to post alert on UI that this needs to be replaced
					// TODO: Add icon on OBS items that are used by application?
				}
			}
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