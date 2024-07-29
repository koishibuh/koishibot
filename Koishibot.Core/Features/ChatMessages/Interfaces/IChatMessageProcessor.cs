using Koishibot.Core.Features.ChatMessages.Models;

namespace Koishibot.Core.Features.ChatMessages.Interfaces;

public interface IChatMessageProcessor
{
	Task Start(ChatMessageDto e);
}
