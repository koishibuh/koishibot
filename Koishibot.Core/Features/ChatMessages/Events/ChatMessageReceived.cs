using Koishibot.Core.Configurations;
using Koishibot.Core.Features.ChatMessages.Interfaces;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.Client.Events;
namespace Koishibot.Core.Features.ChatMessages.Events;

// == ⚫ EVENT SUB == //

public class ChatMessageReceived(
	ILogger<ChatMessageReceived> Log,
	StreamerTwitchClient Client,
	IServiceScopeFactory ScopeFactory
	) : IChatMessageReceived
{
	public void SetupHandler()
	{
		Client.OnMessageReceived += OnChatMessageReceived;
	}

	private async Task OnChatMessageReceived
		(object? sender, OnMessageReceivedArgs args)
	{
		try
		{
			var e = args.ConvertToEvent();

			using var scope = ScopeFactory.CreateScope();
			var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

			await mediatr.Send(new ProcessChatMessageCommand(e));
		}
		catch (Exception ex)
		{
			Log.LogError(ex.ToString());
			//cry
		}
	
	}
}