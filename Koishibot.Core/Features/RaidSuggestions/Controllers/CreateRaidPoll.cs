using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.RaidSuggestions;

// == ⚫ POST == //

public class CreateRaidPollController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Outgoing Raid"])]
	[HttpPost("/api/raid/poll")]
	public async Task<ActionResult> CreateRaidPoll()
	{
		await Mediator.Send(new CreateRaidPollCommands());
		return Ok();
	}
}

// == ⚫ COMMAND  == //

public record CreateRaidPollCommands : IRequest;

// == ⚫ HANDLER  == //

public record CreateRaidPollHandler(
		IOptions<Settings> Settings,
	ILogger<CreateRaidPollHandler> Log,
	ITwitchApiRequest TwitchApiRequest,
	IAppCache Cache, ITwitchIrcService BotIrc, 
	//IRaidSuggestionsApi StartRaidPollApi,
	ISignalrService Signalr
	) : IRequestHandler<CreateRaidPollCommands>
{
	public async Task Handle
					(CreateRaidPollCommands e, CancellationToken cancel)
	{
		await Signalr.SendRaidOverlayStatus(true);

		var raidCandidates = Cache.GetRaidCandidates();

		Log.LogInformation("Waiting for raiding streams to load");

		await Task.Delay(TimeSpan.FromSeconds(20));

		var parameters = new SendAnnouncementRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			ModeratorId = Settings.Value.StreamerTokens.UserId
		};

		var one = raidCandidates[0];
		var two = raidCandidates[1];
		var three = raidCandidates[2];

		var message = $"Here are your raid options to vote on: " +
				$"1. {one.Streamer.Name} - {one.StreamInfo.GameName} ({one.StreamInfo.ViewerCount} Viewers), " +
				$"2. {two.Streamer.Name} - {two.StreamInfo.GameName} ({two.StreamInfo.ViewerCount} Viewers), " +
				$"3. {three.Streamer.Name} - {three.StreamInfo.GameName} ({three.StreamInfo.ViewerCount} Viewers)";

		var body = new SendAnnouncementRequestBody
		{
			Message = message,
		};


		await TwitchApiRequest.SendAnnouncement(parameters, body);

		var choices = raidCandidates.CreatePollChoices();

	
		var pollBody = new CreatePollRequestBody
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			PollTitle = "Who should we raid?",
			Choices = choices,
			DurationInSeconds = 180
		};

		await TwitchApiRequest.CreatePoll(pollBody);

	}
}
