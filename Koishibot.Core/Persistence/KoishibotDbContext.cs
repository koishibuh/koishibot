using Koishibot.Core.Features.ApplicationAuthentication.Models;
using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Features.KoiKinDragons.Models;
using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Persistence;

public class KoishibotDbContext : DbContext
{
	public DbSet<TwitchUser> Users => Set<TwitchUser>();
	public DbSet<Attendance> Attendances => Set<Attendance>();
	public DbSet<YearlyQuarter> YearlyQuarters => Set<YearlyQuarter>();
	public DbSet<ChannelPointRedemption> ChannelPointRedemptions => Set<ChannelPointRedemption>();
	public DbSet<ChannelPointReward> ChannelPointRewards => Set<ChannelPointReward>();
	public DbSet<IncomingRaid> IncomingRaids => Set<IncomingRaid>();
	public DbSet<PollResult> PollResults => Set<PollResult>();
	public DbSet<ChannelFollow> ChannelFollows => Set<ChannelFollow>();
	public DbSet<AppLogin> AppLogins => Set<AppLogin>();
	public DbSet<AppKey> AppKeys => Set<AppKey>();
	public DbSet<DandleResult> DandleResults => Set<DandleResult>();
	public DbSet<DandleWord> DandleWords => Set<DandleWord>();
	public DbSet<ChatCommand> ChatCommands => Set<ChatCommand>();
	public DbSet<TimerGroup> TimerGroups => Set<TimerGroup>();
	public DbSet<CommandName> CommandNames => Set<CommandName>();
	public DbSet<TwitchCheer> Cheers => Set<TwitchCheer>();
	public DbSet<Subscription> Subscriptions => Set<Subscription>();
	public DbSet<GiftSubscription> GiftSubscriptions => Set<GiftSubscription>();
	public DbSet<SupportTotal> SupportTotals => Set<SupportTotal>();
	public DbSet<ObsItem> ObsItems => Set<ObsItem>();
	public DbSet<StreamCategory> StreamCategories => Set<StreamCategory>();
	public DbSet<KoiKinDragon> KoiKinDragons => Set<KoiKinDragon>();
	public DbSet<LiveStream> LiveStreams => Set<LiveStream>();
	public DbSet<StreamSession> StreamSessions => Set<StreamSession>();

	public KoishibotDbContext(DbContextOptions<KoishibotDbContext> options)
		: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(KoishibotDbContext).Assembly);

		//modelBuilder.Entity<TwitchUser>().HasData
	}
}