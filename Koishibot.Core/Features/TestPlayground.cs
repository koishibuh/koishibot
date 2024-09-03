using Koishibot.Core.Services.OBS;

namespace Koishibot.Core.Features;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api")]
public class ATestPlaygroundController : ApiControllerBase
{
	[HttpPost("test")]
	public async Task<ActionResult> TestPlayground()
	{
		var result = await Mediator.Send(new TestPlaygroundCommand());
		return Ok(result);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record TestPlaygroundCommand() : IRequest<string>;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record TestPlaygroundHandler(
ILogger<TestPlaygroundHandler> Log,
ISignalrService Signalr,
IObsService ObsService,
IMediator Mediator
//IAdsApi AdsApi,
//IAppCache Cache,
//// IStreamOnlineHandler StreamOnlineHandler

//KoishibotDbContext Database,
//ISignalrService Signalr,
//IGoogleCalendarApi GoogleCalendarApi
) : IRequestHandler<TestPlaygroundCommand, string>
{
	public async Task<string> Handle
	(TestPlaygroundCommand c, CancellationToken cancel)
	{
		// await ObsService.StartBreak();

		// var time = new TimeSpan(0, 0, 60);
		// var adBreakVm = new AdBreakVm(time, DateTime.Now);
		// await Signalr.SendAdStartedEvent(adBreakVm);
		// return "hi";
		await Task.CompletedTask;

		//var result = await GoogleCalendarApi.GetEvents("8kroeh9nkripcr3iu9m569fe28@group.calendar.google.com");
		////await DandleVoting();


		//var test1 = new OverlayStatus(OverlayName.Dandle, true);

		//await Signalr.SendOverlayStatus(new OverlayStatusVm(test1.Name, test1.Status));

		//await Task.Delay(TimeSpan.FromSeconds(5));

		//await Signalr.SendOverlayStatus(new OverlayStatusVm(test1.Name, false));

		//var letters = new List<LetterInfo>();
		//letters.Add(new LetterInfo("F", "#000000"));
		//letters.Add(new LetterInfo("R", "#008000"));
		//letters.Add(new LetterInfo("O", "#008000"));
		//letters.Add(new LetterInfo("T", "#fff600"));
		//letters.Add(new LetterInfo("H", "#000000"));

		//var keys = new List<LetterInfo>();
		//keys.Add(new LetterInfo("f", "#000000, #000000"));
		//keys.Add(new LetterInfo("r", "#008000, #008000"));
		//keys.Add(new LetterInfo("o", "#008000, #008000"));
		//keys.Add(new LetterInfo("t", "#fff600, #fff600"));
		//keys.Add(new LetterInfo("h", "#000000, #000000"));

		//var vm = new DandleWordGuessVm(letters, keys);
		//await Signalr.SendDandleWordGuess(vm);


		//await Task.Delay(TimeSpan.FromSeconds(5));

		//var letters1 = new List<LetterInfo>();
		//letters1.Add(new LetterInfo("B", "#000000"));
		//letters1.Add(new LetterInfo("R", "#008000"));
		//letters1.Add(new LetterInfo("O", "#008000"));
		//letters1.Add(new LetterInfo("O", "#fff600"));
		//letters1.Add(new LetterInfo("M", "#000000"));

		//var keys1 = new List<LetterInfo>();
		//keys1.Add(new LetterInfo("b", "#000000, #000000"));
		//keys1.Add(new LetterInfo("r", "#008000, #008000"));
		//keys1.Add(new LetterInfo("o", "54deg, #fff600 50%, #008000 50%"));
		//keys1.Add(new LetterInfo("m", "#000000, #000000"));

		//var vm1 = new DandleWordGuessVm(letters1, keys1);
		//await Signalr.SendDandleWordGuess(vm1);


		//var timer = new DandleTimerVm("Suggestions Open", 5, 0);

		//await Signalr.SendDandleTimer(timer);

		//await Task.Delay(TimeSpan.FromSeconds(5));


		//var timer2 = new DandleTimerVm("Now Voting", 2, 0);

		//await Signalr.SendDandleTimer(timer2);

		//var suggestion1 = new DandleWordSuggestionVm(1, "TheBookSnail", "snail");
		//await Signalr.SendDandleWordSuggestion(suggestion1);
		//await Task.Delay(TimeSpan.FromSeconds(5));


		//var suggestion2 = new DandleWordSuggestionVm(1, "Anonnonymous", "glare");
		//await Signalr.SendDandleWordSuggestion(suggestion2);
		//await Task.Delay(TimeSpan.FromSeconds(5));


		//var suggestion3 = new DandleWordSuggestionVm(1, "Spacey3D", "space");
		//await Signalr.SendDandleWordSuggestion(suggestion3);
		//await Task.Delay(TimeSpan.FromSeconds(5));


		//var suggestion4 = new DandleWordSuggestionVm(1, "HonestDanGames", "stomp");
		//await Signalr.SendDandleWordSuggestion(suggestion4);

		//var wordList = new List<LetterInfo>();
		//wordList.Add(new LetterInfo("S", "#000000"));
		//wordList.Add(new LetterInfo("N", "#000000"));
		//wordList.Add(new LetterInfo("A", "#000000"));
		//wordList.Add(new LetterInfo("I", "#000000"));
		//wordList.Add(new LetterInfo("L", "#000000"));
		//var wordGuess = new DandleWordGuessVm(wordList);

		//await Signalr.SendDandleWordGuess(wordGuess);

		//await Task.Delay(TimeSpan.FromSeconds(3));

		//var wordList1 = new List<LetterInfo>();
		//wordList1.Add(new LetterInfo("F", "#000000"));
		//wordList1.Add(new LetterInfo("R", "#008000"));
		//wordList1.Add(new LetterInfo("O", "#008000"));
		//wordList1.Add(new LetterInfo("T", "#fff600"));
		//wordList1.Add(new LetterInfo("H", "#000000"));
		//var wordGuess1 = new DandleWordGuessVm(wordList1);

		//await Signalr.SendDandleWordGuess(wordGuess1);

		//await Task.Delay(TimeSpan.FromSeconds(3));

		////var wordList2 = new List<LetterInfo>();
		////wordList1.Add(new LetterInfo("T", "#008000"));
		////wordList1.Add(new LetterInfo("R", "#008000"));
		////wordList1.Add(new LetterInfo("O", "#008000"));
		////wordList1.Add(new LetterInfo("V", "#008000"));
		////wordList1.Add(new LetterInfo("E", "#008000"));
		////var wordGuess2 = new DandleWordGuessVm(wordList2);

		//await Signalr.SendDandleWordGuess(wordGuess1);

		//await Task.Delay(TimeSpan.FromSeconds(3));

		//await Signalr.SendDandleWordGuess(wordGuess1);

		//await Task.Delay(TimeSpan.FromSeconds(3));

		//await Signalr.SendDandleWordGuess(wordGuess1);
		//await Signalr.SendDandleWordGuess(wordGuess1);


		//await new StreamEventVm()
		//				.CreateFollowEvent("UserOne")
		//				.SendToVue(Signalr);

		//await Task.Delay(TimeSpan.FromSeconds(5));

		//await new StreamEventVm()
		//				.CreateFollowEvent("UserTwo")
		//				.SendToVue(Signalr);

		//var raidpoll3 = new RaidPollVm
		//{
		//	CurrentPollResults = new List<PollChoiceInfo>
		//	{
		//		new("instafluff", 0),
		//		new("ipaintbirbs", 0),
		//		new("tilldays", 0)
		//	}
		//};

		//await raidpoll3.SendToVue(Signalr);

		//var list = new List<RaidCandidateDto>();
		//list.Add(new RaidCandidateDto("instafluff", "User2", "Title2", "Game2"));
		//list.Add(new RaidCandidateDto("ipaintbirbs", "User3", "Title3", "Game3"));
		//list.Add(new RaidCandidateDto("tilldays", "User1", "Title1", "Game1"));

		//var raidVm = new RaidCandidateVm { MultistreamUrl = "url", RaidCandidates = list };
		//await Signalr.SendRaidCandidates(raidVm);

		//await Task.Delay(TimeSpan.FromSeconds(2));

		//await new CurrentTimer()
		//					.SetStartingSoon()
		//					.AddToCache(Cache)
		//					.ConvertToVm()
		//					.UpdateOverlay(Signalr);

		//await Signalr.SendRaidOverlayStatus(true);

		//await Task.Delay(TimeSpan.FromSeconds(15));

		//var raidpoll = new RaidPollVm
		//{
		//	CurrentPollResults = new List<PollChoiceInfo>
		//	{
		//		new("instafluff", 0),
		//		new("ipaintbirbs", 0),
		//		new("tilldays", 0)
		//	}
		//};

		//await raidpoll.SendToVue(Signalr);

		//await Task.Delay(TimeSpan.FromSeconds(5));

		//var raidpoll1 = new RaidPollVm
		//{
		//	CurrentPollResults = new List<PollChoiceInfo>
		//	{
		//		new("instafluff", 1),
		//		new("ipaintbirbs",0),
		//		new("tilldays", 0)
		//	}
		//};

		//await raidpoll1.SendToVue(Signalr);

		//await Task.Delay(TimeSpan.FromSeconds(5));

		//var raidpoll2 = new RaidPollVm
		//{
		//	CurrentPollResults = new List<PollChoiceInfo>
		//	{
		//		new("instafluff", 1),
		//		new("ipaintbirbs", 1),
		//		new("tilldays", 0)
		//	}
		//};

		//await raidpoll2.SendToVue(Signalr);

		//await Task.Delay(TimeSpan.FromSeconds(3));


		//await Signalr.SendRaidOverlayStatus(false);


		//await StreamOnlineHandler.Start(null, new TwitchLib.EventSub.Websockets.Core.EventArgs.Stream.StreamOnlineArgs());
		//var result = await VideoApi.GetRecentVod("98683749");

		//var result1 = await AdsApi.GetAdSchedule();

		//var result2 = await AdsApi.GetAdScheduleMidPomodoro();


		//var choices = new Dictionary<string, int>();
		//choices.Add("Name1", 50);
		//choices.Add("Name2", 10);
		//choices.Add("Name3", 11);

		////return new testVm("Test", choices);
		//var currentPollVm = new PollVm("0", "A test poll", DateTimeOffset.UtcNow, DateTimeOffset.Now, TimeSpan.FromMinutes(3), choices);

		//await Notification.SendPoll(currentPollVm);

		return "done";
	}

	//public async Task DandleVoting()
	//{
	//	var result = new List<PollChoiceInfo>();
	//	result.Add(new PollChoiceInfo("snail", 0));
	//	result.Add(new PollChoiceInfo("skink", 0));
	//	result.Add(new PollChoiceInfo("plant", 0));
	//	await Signalr.SendDandleGuessChoices(result);

	//	await Task.Delay(TimeSpan.FromSeconds(2));

	//	var vote = new PollChoiceInfo("snail", 1);
	//	await Signalr.SendDandleVote(vote);

	//	await Task.Delay(TimeSpan.FromSeconds(2));
	//	var vote2 = new PollChoiceInfo("snail", 1);
	//	await Signalr.SendDandleVote(vote2);

	//	await Task.Delay(TimeSpan.FromSeconds(2));
	//	var vote3 = new PollChoiceInfo("plant", 1);
	//	await Signalr.SendDandleVote(vote3);

	//	await Task.Delay(TimeSpan.FromSeconds(2));
	//	var vote4 = new PollChoiceInfo("skink", 1);
	//	await Signalr.SendDandleVote(vote4);

	//	await Task.Delay(TimeSpan.FromSeconds(2));
	//	var vote5 = new PollChoiceInfo("snail", 1);
	//	await Signalr.SendDandleVote(vote5);
	//}
}

public record testVm(
string Name,
Dictionary<string, int> Choices);