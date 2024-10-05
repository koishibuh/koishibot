using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

namespace Koishibot.Core.Features.Moderation;

// == ⚫ COMMAND == //

public record ChatClearedCommand
	(ChatClearedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChatClearedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChatClearedCommand>
{
	public async Task Handle(ChatClearedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: Create alert on UI
	}
}