using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.RaidSuggestions.Interfaces;

public interface IRaidSuggestionValidation
{
    Task Start(TwitchUser suggestedBy, string suggestedStreamer);
}