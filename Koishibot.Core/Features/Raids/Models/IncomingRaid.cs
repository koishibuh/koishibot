using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Raids.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class IncomingRaid
{
	public int Id { get; set; }
	// public int TwitchStreamId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int RaidedByUserId { get; set; }
	public int ViewerCount { get; set; }
	// public TwitchStream TwitchStream { get; set; } = null!;
	public TwitchUser RaidedByUser { get; set; } = null!;

	// public StreamSession StreamSession { get; set; }
	// public int StreamSessionId { get; set; }

	//

	public IncomingRaid Set(int streamSessionId, int userId, int viewerCount)
	{
		// TwitchStreamId = streamSessionId;
		Timestamp = DateTimeOffset.UtcNow;
		RaidedByUserId = userId;
		ViewerCount = viewerCount;
		return this;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class IncomingRaidConfig : IEntityTypeConfiguration<IncomingRaid>
{
	public void Configure(EntityTypeBuilder<IncomingRaid> builder)
	{
		builder.ToTable("IncomingRaids");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		// builder.Property(p => p.TwitchStreamId);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.RaidedByUserId);

		builder.Property(p => p.ViewerCount);

		builder.HasOne(p => p.RaidedByUser)
		.WithMany(p => p.RaidsFromThisUser)
		.HasForeignKey(p => p.RaidedByUserId)
		.IsRequired();

		// builder.HasOne(p => p.TwitchStream)
		// 	.WithMany(p => p.IncomingRaids)
		// 	.HasForeignKey(p => p.TwitchStreamId)
		// 	.IsRequired();
	}
}