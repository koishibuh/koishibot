namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

public interface IRewardRedeemed
{
    Task SetupHandler();
    Task SubToEvent();
}
