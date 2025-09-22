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
	//TODO: update responses to use database
	public async Task ProcessVote(ChatMessageDto c)
	{
		int index;
		var isNumber = int.TryParse(c.Message, out index);

		if (index is > 3 or < 1)
		{
			var data = new { User = c.User.Name, Number = c.Message };
			await ChatReplyService.CreateResponse(Response.InvalidVote, data);

			Log.LogInformation("Not a valid vote number"); 
			return;
		}

		index = index - 1;

		var dandleInfo = Cache.GetDandleInfo();

		if (dandleInfo.UserAlreadyVoted(c.User, index))
		{
			var data = new { User = c.User.Name };
			await ChatReplyService.CreateResponse(Response.AlreadyVoted, data);
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

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDandleVoteProcessor
{
	Task ProcessVote(ChatMessageDto c);
}