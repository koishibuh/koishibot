using Koishibot.Core.Configurations;
using Koishibot.Core.Features.ChatMessages.Interfaces;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.Client.Events;
namespace Koishibot.Core.Features.ChatMessages.Events;

// == ⚫ EVENT SUB == //

/// <summary>
/// This is triggered when a message is sent through ChatMessageService using the <br/>
/// Client.SendMessageAsync method. This is not triggered if a message is written directly<br/>
/// in the TwitchChat
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
/// <returns></returns>
public class ChatMessageSent(
	StreamerTwitchClient Client,
	IServiceScopeFactory ScopeFactory
	) : IChatMessageSent
{
	public void SetupHandler()
	{
		Client.OnMessageSent += OnChatMessageSent;
	}

	private async Task OnChatMessageSent
		(object? sender, OnMessageSentArgs args)
	{
		var e = args.ConvertToEvent();

		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new ProcessChatMessageCommand(e));
	}
}