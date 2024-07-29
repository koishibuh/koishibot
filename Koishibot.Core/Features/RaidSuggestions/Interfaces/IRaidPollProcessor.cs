using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;

namespace Koishibot.Core.Features.RaidSuggestions.Interfaces;

public interface IRaidPollProcessor
{
	Task Start(PollEndedEvent e);
}