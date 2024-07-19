namespace Koishibot.Core.Features.AdBreak.Interfaces;

public interface IAdBreakStarted
{
    Task SetupHandler();
    Task SubToEvent();
}
