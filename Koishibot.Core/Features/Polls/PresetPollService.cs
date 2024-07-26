//using Koishibot.Core.Features.Polls.Enums;
//using Koishibot.Core.Features.Polls.Interfaces;
//using Koishibot.Core.Features.Polls.Models;
//namespace Koishibot.Core.Features.Polls;

//// == ⚫ == //

//public record PresetPollService(
//	ITwitchPollApi PollApi
//	) : IPresetPollService
//{
//	public async Task Start(PollType pollType, string username)
//	{
//		switch (pollType)
//		{
//			case PollType.DadJoke:
//				var poll = new PendingPoll();
//				poll.CreateDadJokePoll(username);
//				await PollApi.StartPoll(poll);
//				break;

//			default: return;
//		}
//	}
//}