namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamOnline
{
    Task SetupMethod();
    Task SubToEvent();
}
