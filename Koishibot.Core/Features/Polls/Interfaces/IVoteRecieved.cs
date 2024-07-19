namespace Koishibot.Core.Features.Polls.Interfaces;

public interface IVoteRecieved
{
	Task SetupHandler();
	Task SubToEvent();
}
