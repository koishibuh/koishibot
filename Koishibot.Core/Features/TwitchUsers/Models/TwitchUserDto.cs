namespace Koishibot.Core.Features.TwitchUsers.Models;

public record TwitchUserDto(
string TwitchId,
string Login,
string Name
)
{
	public bool IsIgnoredUser() => Login is "koishibuh" or "streamelements";
}