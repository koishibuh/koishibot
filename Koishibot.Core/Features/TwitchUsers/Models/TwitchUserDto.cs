namespace Koishibot.Core.Features.TwitchUsers.Models;

public record TwitchUserDto(
		string TwitchId,
		string Login,
		string Name
)
{
	public bool IsIgnoredUser()
	{
		// Todo: Tolower
		return Name == "koishibuh" || Name == "StreamElements";
	}
}
