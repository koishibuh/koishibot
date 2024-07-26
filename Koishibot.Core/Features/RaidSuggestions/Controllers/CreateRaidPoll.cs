//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using Koishibot.Core.Features.RaidSuggestions.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using TwitchLib.Api.Helix.Models.Polls.CreatePoll;
//namespace Koishibot.Core.Features.RaidSuggestions;

//// == ⚫ POST == //

//public class CreateRaidPollController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Outgoing Raid"])]
//	[HttpPost("/api/raid/poll")]
//	public async Task<ActionResult> CreateRaidPoll()
//	{
//		await Mediator.Send(new CreateRaidPollCommands());
//		return Ok();
//	}
//}

//// == ⚫ COMMAND  == //

//public record CreateRaidPollCommands : IRequest;

//// == ⚫ HANDLER  == //

//public record CreateRaidPollHandler(
//	ILogger<CreateRaidPollHandler> Log,
//	IAppCache Cache, IRaidSuggestionsApi StartRaidPollApi,
//	ISignalrService Signalr
//	)	: IRequestHandler<CreateRaidPollCommands>
//{
//	public async Task Handle
//					(CreateRaidPollCommands e, CancellationToken cancel)
//	{
//		await Signalr.SendRaidOverlayStatus(true);

//		var raidCandidates = Cache.GetRaidCandidates();

//		Log.LogInformation("Waiting for raiding streams to load");

//		await Task.Delay(TimeSpan.FromSeconds(20));

//		await StartRaidPollApi.AnnounceRaidCandidateOptions(raidCandidates);

//		var choices = raidCandidates.CreatePollChoices();

//		await StartRaidPollApi.StartPoll("Who should we raid?", choices, 180);
//	}
//}

//// == ⚫ TWITCH API  == //

//public partial record RaidSuggestionsApi : IRaidSuggestionsApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-poll">Create Poll Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<string?> StartPoll
//		(string title, List<string> choiceList, int duration)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var choices = choiceList
//			.Select(title => new Choice { Title = title })
//			.ToArray();

//		var poll = new CreatePollRequest
//		{
//			BroadcasterId = StreamerId,
//			Title = title,
//			Choices = choices,
//			DurationSeconds = duration
//		};

//		var result = await TwitchApi.Helix.Polls.CreatePollAsync(poll);
//		return result is null || result.Data.Length == 0
//			? throw new Exception("Error while trying to Create A Poll via Api")
//			: result.Data[0].Id;
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-announcement">Send Announcement Documentation</see>
//	/// Case-sensitive Colors: blue, green, orange, purple, primary (default)
//	/// </summary>
//	/// <returns></returns>
//	public async Task AnnounceRaidCandidateOptions
//		(List<RaidSuggestion> raidCandidates)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var one = raidCandidates[0];
//		var two = raidCandidates[1];
//		var three = raidCandidates[2];

//		var message = $"Here are your raid options to vote on: " +
//				$"1. {one.Streamer.Name} - {one.StreamInfo.GameName} ({one.StreamInfo.ViewerCount} Viewers), " +
//				$"2. {two.Streamer.Name} - {two.StreamInfo.GameName} ({two.StreamInfo.ViewerCount} Viewers), " +
//				$"3. {three.Streamer.Name} - {three.StreamInfo.GameName} ({three.StreamInfo.ViewerCount} Viewers)";

//		await TwitchApi.Helix.Chat.SendChatAnnouncementAsync
//			(StreamerId, StreamerId, message);
//	}
//}