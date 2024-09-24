using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.TwitchUsers.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class TwitchUser
{
	public int Id { get; set; }
	public string TwitchId { get; set; } = null!;
	public string Login { get; set; } = string.Empty;
	public string Name { get; set; } = null!;
	public string Permissions { get; set; } = PermissionLevel.Everyone;

	// NAVIGATION
	public Attendance? Attendance { get; set; }
	public IList<OutgoingRaid> RaidsSuggestedByThisUser { get; set; } = [];
	public IList<OutgoingRaid> UsersSuggestingThisRaidTarget { get; set; } = [];
	public IList<IncomingRaid> RaidsFromThisUser { get; set; } = [];
	public IList<ChannelPointRedemption> RedeemedChannelPointRewards { get; set; } = [];
	public IList<ChannelFollow> ChannelFollows { get; set; } = [];
	public IList<TwitchCheer> Cheers { get; set; } = [];
	public IList<Subscription> Subscriptions { get; set; } = [];
	public IList<GiftSubscription> GiftedSubscriptions { get; set; } = [];
	public IList<Kofi> KofiSupport { get; set; } = [];
	public IList<SETip> SETips { get; set; } = [];
	public SupportTotal? SupportTotal { get; set; }
	public WordpressItemTag? WordpressItemTag { get; set; }

	// == ⚫ == //
	public bool IsIgnored() => Permissions == PermissionLevel.Ignore;
	public bool ChangedUsername(string currentName) => currentName != Name;


	public TwitchUser UpdateUserInfo(TwitchUserDto currentUser)
	{
		Name = currentUser.Name;
		Login = currentUser.Login;

		return this;
	}

	public TwitchUser UpgradePermissions()
	{
		Permissions = PermissionLevel.Koi;
		return this;
	}

	public TwitchUser Initialize(TwitchUserDto userDto)
	{
		TwitchId = userDto.TwitchId;
		Login = userDto.Login;
		Name = userDto.Name;
		Permissions = PermissionLevel.Everyone;
		return this;
	}

	public TwitchUser Set(UserInfo userDto)
	{
		TwitchId = userDto.TwitchId;
		Login = userDto.Login;
		Name = userDto.Name;
		return this;
	}

	public TwitchUser Set(UserData userDto)
	{
		TwitchId = userDto.Id;
		Login = userDto.Login;
		Name = userDto.Name;
		Permissions = PermissionLevel.Everyone;
		return this;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class UserConfig : IEntityTypeConfiguration<TwitchUser>
{
	public void Configure(EntityTypeBuilder<TwitchUser> builder)
	{
		builder.ToTable("Users");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.TwitchId);

		builder.HasIndex(p => p.TwitchId)
		.IsUnique();

		builder.Property(p => p.Login);

		builder.Property(p => p.Name);

		builder.Property(p => p.Permissions);

		builder.HasOne(p => p.Attendance)
		.WithOne(p => p.User)
		.HasForeignKey<Attendance>(p => p.UserId);

		builder.HasMany(p => p.ChannelFollows)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired();

		builder.HasMany(p => p.Cheers)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired();

		builder.HasMany(p => p.Subscriptions)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired();

		builder.HasMany(p => p.GiftedSubscriptions)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired();

		builder.HasOne(p => p.SupportTotal)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey<SupportTotal>(p => p.UserId);

		builder.HasMany(p => p.KofiSupport)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired(false);

		builder.HasMany(p => p.SETips)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired(false);

		builder.HasOne(p => p.WordpressItemTag)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey<WordpressItemTag>(p => p.UserId)
		.IsRequired(false);
	}
}