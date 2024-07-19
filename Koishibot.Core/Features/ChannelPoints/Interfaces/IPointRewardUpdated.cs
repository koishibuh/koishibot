namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

public interface IPointRewardUpdated
{
    Task SetupHandler();
    Task SubToEvent();
}