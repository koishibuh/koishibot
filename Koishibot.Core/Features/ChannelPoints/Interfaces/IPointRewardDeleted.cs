namespace Koishibot.Core.Features.ChannelPoints.Interfaces;

public interface IPointRewardDeleted
{
	Task SetupHandler();
	Task SubToEvent();
}