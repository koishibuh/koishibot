using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.StreamInformation.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class StreamSession : IEntity
{
	public int Id { get; set; }
	public TimeSpan Duration { get; set; }
	public bool AttendanceMandatory { get; set; }

	public int YearlyQuarterId { get; set; }
	public YearlyQuarter YearlyQuarter { get; set; } = null!;

	public List<LiveStream> LiveStreams { get; set; } = [];
	public OutgoingRaid? OutgoingRaid { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class StreamSessionConfig : IEntityTypeConfiguration<StreamSession>
{
	public void Configure(EntityTypeBuilder<StreamSession> builder)
	{
		builder.ToTable("StreamSessions");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Duration);
		builder.Property(p => p.AttendanceMandatory);

		builder.HasMany(p => p.LiveStreams)
		.WithOne(p => p.StreamSession)
		.HasForeignKey(p => p.StreamSessionId)
		.IsRequired();

		builder.HasOne(p => p.OutgoingRaid)
		.WithOne(p => p.StreamSession)
		.HasForeignKey<OutgoingRaid>(p => p.StreamSessionId)
		.IsRequired();
	}
}