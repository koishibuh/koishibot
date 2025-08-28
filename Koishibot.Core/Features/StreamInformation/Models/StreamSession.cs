using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence;
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

	public string Summary { get; set; } = "";

	public List<LiveStream> LiveStreams { get; set; } = [];
	public OutgoingRaid? OutgoingRaid { get; set; }

	public StreamSession ResetDuration()
	{
		Duration = TimeSpan.Zero;
		return this;
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class StreamSessionExtensions
{
	public static async Task<StreamSession?> GetRecentStreamSession(this KoishibotDbContext database)
	{
		return await database.StreamSessions
			.OrderByDescending(x => x.Id)
			.FirstOrDefaultAsync();
	}
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
		builder.Property(p => p.Summary);

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