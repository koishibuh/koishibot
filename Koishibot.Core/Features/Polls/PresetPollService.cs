using Koishibot.Core.Features.Polls.Enums;
using Koishibot.Core.Features.Polls.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Polls;

// == ⚫ == //

public record PresetPollService(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
	) : IPresetPollService
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Start(PollType pollType, string username)
	{
		switch (pollType)
		{
			case PollType.DadJoke:
				var body = CreateDadJokeRequestBody(StreamerId, username);
				await TwitchApiRequest.CreatePoll(body);
				break;

			default: return;
		}
	}

	public CreatePollRequestBody CreateDadJokeRequestBody(string streamerId, string username)
	{
		var choices = new List<string> { "⭐⭐⭐⭐⭐", "⭐⭐⭐⭐", "⭐⭐⭐", "⭐⭐", "⭐" };

		return new CreatePollRequestBody
		{
			BroadcasterId = streamerId,
			PollTitle = $"How would you rate {username}'s Dad Joke?",
			Choices = choices,
			DurationInSeconds = 120
		};
	}
}