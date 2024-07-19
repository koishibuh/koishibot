using Microsoft.AspNetCore.SignalR;
namespace Koishibot.Core.Services.SignalR;

public class SignalrHub : Hub<ISignalrHub>
{
	public override async Task OnConnectedAsync()
	{
		await Clients.Client(Context.ConnectionId).ReceiveNotification(
			$"SignalR is now connected");

		await base.OnConnectedAsync();
	}

	// Clients - used to invoke methods on clients connected to this hub
	// Groups - an abstraction for adding and removing connections from groups
	// Context - used for accessing information about the hub caller connection 
}