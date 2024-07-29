using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //

public record UserChatHistoryClearedCommand
	(UserMessagesClearedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record UserChatHistoryClearedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<UserChatHistoryClearedCommand>
{
	public async Task Handle(UserChatHistoryClearedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: Cross out messages, create alert
	}
}