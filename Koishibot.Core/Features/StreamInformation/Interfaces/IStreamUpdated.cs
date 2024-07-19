namespace Koishibot.Core.Features.StreamInformation.Interfaces;

public interface IStreamUpdated
{
	Task SetupHandler();
	Task SubToEvent();
}