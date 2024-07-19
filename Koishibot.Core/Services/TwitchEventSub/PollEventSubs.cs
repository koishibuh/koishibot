using Koishibot.Core.Features.Polls.Interfaces;
namespace Koishibot.Core.Services.TwitchEventSub;

public record PollEventSubs(
	IOptions<Settings> Settings,
	IPollStarted PollStarted,
	IVoteRecieved VoteRecieved,
	IPollEnded PollEnded
	) : IPollEventSubs
{
	// == ⚫ == //

	public async Task SetupAndSubscribe()
	{
		if (Settings.Value.DebugMode)
		{
			return;
		}

		await PollStarted.SetupHandler();
		await VoteRecieved.SetupHandler();
		await PollEnded.SetupHandler();
	}

	// == ⚫ == //

	public async Task SubscribeToEvents()
	{
		if (Settings.Value.DebugMode)
		{
			return;

		}
		await PollStarted.SubToEvent();
		await VoteRecieved.SubToEvent();
		await PollEnded.SubToEvent();
	}
}