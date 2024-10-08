using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //

public record ChatMessageDeletedCommand
	(MessageDeletedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChatMessageDeletedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChatMessageDeletedCommand>
{
	public async Task Handle(ChatMessageDeletedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO: Cross out message, create alert
	}
}