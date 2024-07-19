using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.Supports.Models;

namespace Koishibot.Core.Features.TwitchUsers.Models;

public class TwitchUser
{
	public int Id { get; set; }
	public string TwitchId { get; set; } = null!;
	public string Login { get; set; } = string.Empty;
	public string Name { get; set; } = null!;

	// NAVIGATION

	public Attendance? Attendance { get; set; }
	public IList<OutgoingRaid> RaidsSuggestedByThisUser { get; set; } = [];
	public IList<OutgoingRaid> UsersSuggestingThisRaidTarget { get; set; } = [];
	public IList<IncomingRaid> RaidsFromThisUser { get; set; } = [];
	public IList<ChannelPointRedemption> RedeemedChannelPointRewards { get; set; } = [];
	public IList<ChannelFollow> ChannelFollows { get; set; } = [];
	public IList<TwitchCheer> Cheers { get; set; } = [];
	//public IList<Subscription> Subscriptions { get; set; } = [];
	//public IList<GiftSubscription> GiftedSubscriptions { get; set; } = [];

	// == ⚫ == //

	public bool IsOnBlocklist()
	{
		return Login == "koishibuh" || Login == "honestdanbot";
	}

	public bool ChangedUsername(string currentName)
	{
		return currentName != Name;
	}

	public TwitchUser UpdateUserInfo(TwitchUserDto currentUser)
	{
		Name = currentUser.Name;
		Login = currentUser.Login;

		return this;
	}

	public TwitchUser Initialize(TwitchUserDto userDto)
	{
		TwitchId = userDto.TwitchId;
		Login = userDto.Login;
		Name = userDto.Name;
		return this;
	}

	public TwitchUser Set(UserInfo userDto)
	{
		TwitchId = userDto.TwitchId;
		Login = userDto.Login;
		Name = userDto.Name;
		return this;
	}
}