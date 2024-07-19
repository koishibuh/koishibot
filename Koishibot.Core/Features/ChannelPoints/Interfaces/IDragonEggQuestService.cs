using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

public interface IDragonEggQuestService
{
    Task Initialize();
    Task GetResult(TwitchUser user, DateTimeOffset redeemedAt);
}