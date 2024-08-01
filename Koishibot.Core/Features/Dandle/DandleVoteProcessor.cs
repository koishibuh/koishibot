using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
namespace Koishibot.Core.Features.Dandle;

public record DandleVoteProcessor(
	ILogger<DandleVoteProcessor> Log,
	IChatReplyService ChatReplyService,
	IAppCache Cache, ISignalrService Signalr
	) : IDandleVoteProcessor
{
	public async Task ProcessVote(ChatMessageDto c)
	{
		int index;
		var isNumber = int.TryParse(c.Message, out index);

		if (index > 3 || index < 1)
		{
			var data = new UserNumberData(c.User.Name, c.Message);
			await ChatReplyService.App(Command.InvalidVote, data);

			Log.LogInformation("Not a valid vote number");
			return;
		}

		index = index - 1;

		var dandleInfo = Cache.GetDandleInfo();

		if (dandleInfo.UserAlreadyVoted(c.User, index))
		{
			var data = new UsernameData(c.User.Name);
			await ChatReplyService.App(Command.AlreadyVoted, data);
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