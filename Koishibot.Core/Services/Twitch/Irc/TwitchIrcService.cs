using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Irc.Extensions;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Services.Twitch.Irc;

public record TwitchIrcService(
		IOptions<Settings> Settings,
		IAppCache Cache,
		IServiceScopeFactory ScopeFactory,
		ISignalrService SignalrService
		//IRefreshAccessTokenService TokenProcessor
		) : ITwitchIrcService
{
	public CancellationToken? Cancel { get; set; }
	//public TwitchIrcWebSocket? StreamerIrc { get; set; }
	public TwitchIrcWebSocket? BotIrc { get; set; }

	public async Task CreateWebSocket(CancellationToken cancel)
	{
		Cancel ??= cancel;
		//StreamerIrc ??= new TwitchIrcWebSocket(
		//		"wss://irc-ws.chat.twitch.tv:443", Cancel.Value, Settings.Value.StreamerTokens.Username, 3);

		BotIrc ??= new TwitchIrcWebSocket(
				"wss://irc-ws.chat.twitch.tv:443", Cancel.Value, Settings.Value.StreamerTokens.Username, 3);

		//await TokenProcessor.EnsureValidToken();

		var streamer = Settings.Value.StreamerTokens;
		var bot = Settings.Value.BotTokens;

		//StreamerIrc.Connected += async () => await OnConnected("streamer", streamer.AccessToken, streamer.Username);
		//StreamerIrc.MessageReceived += async message => await OnMessageReceived("streamer", message);
		//StreamerIrc.Reconnecting += async () => await OnReconnecting();
		//StreamerIrc.OnDisconnectError += async () => await OnDisconnectError();

		//await StreamerIrc.Connect();

		BotIrc.Connected += async () => await OnConnected("bot", bot.AccessToken, bot.Username);
		BotIrc.MessageReceived += async message => await OnMessageReceived("bot", message);
		BotIrc.Reconnecting += async () => await OnReconnecting();
		BotIrc.OnDisconnectError += async () => await OnDisconnectError();

		await BotIrc.Connect();
	}

	public async Task OnConnected(string irc, string accessToken, string username)
	{
		//if (StreamerIrc is null) { throw new Exception(); }
		if (BotIrc is null) { throw new Exception(); }

		//if (irc is "streamer")
		//{
		//	await SignalrService.SendLog(new LogVm("Connecting to Twitch Chat", "Info"));
		//	await SignalrService.SendLog(new LogVm("Connection to Twitch Chat sucessful", "Info"));
		//	await StreamerIrc.SendMessage($"CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
		//	await StreamerIrc.SendMessage($"PASS oauth:{accessToken}");
		//	await StreamerIrc.SendMessage($"NICK {username}");

		//	await Task.Delay(3000);
		//	await StreamerIrc.JoinChannel();
		//	await StreamerIrc.SendMessageToChat("Joined channel");
		//}
		//else
		//{
		await SignalrService.SendLog(new LogVm("Connecting to Twitch Chat", "Info"));
		await SignalrService.SendLog(new LogVm("Connection to Twitch Chat sucessful", "Info"));
		await BotIrc.SendMessage($"CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
		await BotIrc.SendMessage($"PASS oauth:{accessToken}");
		await BotIrc.SendMessage($"NICK {username}");

		await Task.Delay(3000);
		await BotIrc.JoinChannel();
		await BotIrc.SendMessageToChat("Joined channel");

		await Cache.UpdateServiceStatus(ServiceName.BotIrc, true);
		//}
	}

	public async Task OnMessageReceived(string irc, string message)
	{

		if (message.IsPingMessage())
		{
			await BotIrc!.SendMessage("PONG :tmi.twitch.tv");
		}

		//	return;
		//}

		//if (message.IsPingMessage())
		//{	
		//	await SignalrService.SendLog(new LogVm("PING received", "Info"));
		//	await StreamerIrc!.SendMessage("PONG :tmi.twitch.tv");
		//	await SignalrService.SendLog(new LogVm("PONG sent", "Info"));
		//}
		//else
		//{
		//	//var chatMessage = message.ToTwitchMessage();


		//	using var scope = ScopeFactory.CreateScope();
		//	var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		//	await mediatr.Send(new ProcessChatMessageCommand(message));
		//}
		// Send message to chat processor
	}

	public async Task BotSend(string message)
	{
		if (BotIrc is not null)
		{
			await BotIrc.SendMessage(message);
		}
	}

	//public async Task StreamerSend(string message)
	//{
	//	if (StreamerIrc is not null)
	//	{
	//		await StreamerIrc.SendMessage(message);
	//	}
	//}


	public async Task OnReconnecting()
	{
		await SignalrService.SendLog(new LogVm("Reconnecting to Twitch Chat", "Info"));
	}



	public async Task OnDisconnectError()
	{
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, false);
		await SignalrService.SendLog(new LogVm("Twitch IRC disconnected", "Warning"));
	}
}