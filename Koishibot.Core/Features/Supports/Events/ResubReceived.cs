﻿using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;
namespace Koishibot.Core.Features.Supports.Events;


// == ⚫ COMMAND == //

public record ResubReceivedCommand
	(ResubMessageEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ResubReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ResubReceivedCommand>
{
	public async Task Handle(ResubReceivedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}