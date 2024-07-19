namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

public interface IPointRewardCreated
{
    Task SetupHandler();
    Task SubToEvent();
}