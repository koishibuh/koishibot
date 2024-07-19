using Koishibot.Core.Features.Polls.Enums;
namespace Koishibot.Core.Features.Polls.Interfaces;

public interface IPresetPollService
{
	Task Start(PollType pollType, string username);
}