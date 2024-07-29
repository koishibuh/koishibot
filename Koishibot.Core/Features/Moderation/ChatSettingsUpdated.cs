using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatSettings;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //

public record ChatSettingsUpdatedCommand
	(ChatSettingsUpdatedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChatSettingsUpdatedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ShieldModeStartedCommand>
{
	public async Task Handle(ShieldModeStartedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: alert on UI
	}
}