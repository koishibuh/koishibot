namespace Koishibot.Core.Features.Polls.Interfaces;

public interface IPollStarted
{
    Task SetupHandler();
    Task SubToEvent();
}
