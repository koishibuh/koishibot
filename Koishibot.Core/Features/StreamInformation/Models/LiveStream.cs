using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.StreamInformation.Models;

// Session from Twitch, when stream goes online and offline
/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class LiveStream : IEntity
{
	public int Id { get; set; }
	public string TwitchId { get; set; } // TwitchVideoId
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset? EndedAt { get; set; }

	public int StreamSessionId { get; set; }
	public StreamSession StreamSession { get; set; }

	public bool GracePeriodElapsed(int time, DateTimeOffset startedAt)
		=> (EndedAt + TimeSpan.FromMinutes(time)) < startedAt;
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class LiveStreamConfig : IEntityTypeConfiguration<LiveStream>
{
	public void Configure(EntityTypeBuilder<LiveStream> builder)
	{
		builder.ToTable("LiveStreams");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchId);

		builder.HasIndex(p => p.TwitchId)
		.IsUnique();

		builder.Property(p => p.StartedAt);
		builder.Property(p => p.EndedAt);
	}
}