using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.AspNetCore.SignalR;
namespace Koishibot.Core.Services.SignalR;

public class SignalrHub(IAppCache cache) : Hub<ISignalrHub>
{
	private IAppCache Cache { get; set; } = cache;

	public override async Task OnConnectedAsync()
	{
		await Clients.Client(Context.ConnectionId).ReceiveLog
			(new LogVm($"SignalR is now connected", "i"));

		await Cache.UpdateServiceStatus(ServiceName.SignalR, Status.Online);

		await base.OnConnectedAsync();
	}

	// Clients - used to invoke methods on clients connected to this hub
	// Groups - an abstraction for adding and removing connections from groups
	// Context - used for accessing information about the hub caller connection 
}