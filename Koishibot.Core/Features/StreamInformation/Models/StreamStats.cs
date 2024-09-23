using Koishibot.Core.Features.RaidSuggestions.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.StreamInformation.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class StreamStats
{
	public int Id { get; set; }
	public int StreamSessionId { get; set; }
	public string StreamTitle { get; set; }
	public int StreamCategoryId { get; set; }
	public int ViewerCount { get; set; }

	public StreamCategory StreamCategory { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class StreamStatsConfig : IEntityTypeConfiguration<StreamStats>
{
	public void Configure(EntityTypeBuilder<StreamStats> builder)
	{
		builder.ToTable("StreamStats");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StreamSessionId);
		builder.Property(p => p.StreamTitle);
		builder.Property(p => p.StreamCategoryId);
		builder.Property(p => p.ViewerCount);
	}
}