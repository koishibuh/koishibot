using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.Dandle;

public record DandleVoteProcessor(
	ILogger<DandleVoteProcessor> Log,
	ITwitchIrcService BotIrc,
	IAppCache Cache, ISignalrService Signalr
	) : IDandleVoteProcessor
{
	public async Task ProcessVote(ChatMessageDto c)
	{
		int index;
		var isNumber = int.TryParse(c.Message, out index);

		if (index > 3 || index < 1)
		{
			await BotIrc.BotSend("Not a valid vote number");
			Log.LogInformation("Not a valid vote number");
			return;
		}

		index = index - 1;

		var dandleInfo = Cache.GetDandleInfo();

		if (dandleInfo.UserAlreadyVoted(c.User, index))
		{
			await BotIrc.BotSend($"{c.User.Name}, you have already voted");
			Log.LogInformation("User has already voted");
			return;
		}

		//var user = dandleInfo.GetDandleUser(c.User);

		dandleInfo.AddVote(c.User, index);

		var vm = dandleInfo.CreateVoteVm(index);
		await Signalr.SendDandleVote(vm);
		Cache.UpdateDandle(dandleInfo);
	}
}

public interface IDandleVoteProcessor
{
	Task ProcessVote(ChatMessageDto c);
}