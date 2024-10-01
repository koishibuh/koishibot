using Koishibot.Core.Features.Polls.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Polls;

/*═══════════════════【 SERVICE 】═══════════════════*/
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

	private CreatePollRequestBody CreateDadJokeRequestBody(string streamerId, string username)
	{
		var choices = new List<string> { "⭐⭐⭐⭐⭐", "⭐⭐⭐⭐", "⭐⭐⭐", "⭐⭐", "⭐" };

		return new CreatePollRequestBody
		{
			BroadcasterId = streamerId,
			PollTitle = $"How would you rate {username}'s Dad Joke?",
			Choices = choices
				.Select(x => new ChoiceTitle { Title = x })
				.ToList(),
			DurationInSeconds = 120
		};
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IPresetPollService
{
	Task Start(PollType pollType, string username);
}