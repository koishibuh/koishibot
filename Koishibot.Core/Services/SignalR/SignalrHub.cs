﻿using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.AspNetCore.SignalR;
namespace Koishibot.Core.Services.SignalR;

public class SignalrHub : Hub<ISignalrHub>
{
	public IAppCache _cache { get; set; }

	public SignalrHub(IAppCache Cache)
	{
		_cache = Cache;
	}

	public override async Task OnConnectedAsync()
	{
		await Clients.Client(Context.ConnectionId).ReceiveLog(
			new LogVm($"SignalR is now connected", "i"));

		await _cache.UpdateServiceStatus(ServiceName.SignalR, ServiceStatusString.Online);

		await base.OnConnectedAsync();
	}

	// Clients - used to invoke methods on clients connected to this hub
	// Groups - an abstraction for adding and removing connections from groups
	// Context - used for accessing information about the hub caller connection 
}