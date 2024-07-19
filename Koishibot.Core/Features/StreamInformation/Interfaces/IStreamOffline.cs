namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamOffline
{
    Task SetupMethod();
    Task SubToEvent();
}
