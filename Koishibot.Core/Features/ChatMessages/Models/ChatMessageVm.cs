namespace Koishibot.Core.Features.ChatMessages.Models;

public record ChatMessageVm(
		string UserId,
		string Username,
		List<KeyValuePair<string, string>> Badges,
		string Color,
		string Message) : INotification;