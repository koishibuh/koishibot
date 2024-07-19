namespace Koishibot.Core.Features.Common.Models;

public record UserInfo(
		string TwitchId,
		string Login,
		string Name,
		string BroadcasterType,
		string ChannelDescription,
		string ProfilePictureUrl)
{
};