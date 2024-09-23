using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChannelFollow
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public ChannelFollow Initialize(TwitchUser user)
	{
		Timestamp = DateTimeOffset.UtcNow;
		TwitchUser = user;
		return this;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ChannelFollowConfig : IEntityTypeConfiguration<ChannelFollow>
{
	public void Configure(EntityTypeBuilder<ChannelFollow> builder)
	{
		builder.ToTable("ChannelFollow");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);
	}
}