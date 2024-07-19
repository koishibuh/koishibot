using Koishibot.Core.Configurations;

namespace Koishibot.Core.Services.TwitchIrcClient;

public record ChatMessageService(
	BotTwitchClient BotClient,
	StreamerTwitchClient StreamerClient
	) : IChatMessageService
{
	public async Task BotSend(string message)
	{
		await BotClient.SendMessageAsync("elysiagriffin", message);
	}

	public async Task StreamerSend(string message)
	{
		await StreamerClient.SendMessageAsync("elysiagriffin", message);
	}
}
