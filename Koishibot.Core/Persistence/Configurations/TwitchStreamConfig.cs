using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class TwitchStreamConfig : IEntityTypeConfiguration<TwitchStream>
{
	public void Configure(EntityTypeBuilder<TwitchStream> builder)
	{
		builder.ToTable("TwitchStreams");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StreamId);

		builder.HasIndex(p => p.StreamId)
			.IsUnique();

		builder.Property(p => p.StartedAt);

		builder.Property(p => p.Duration);

		builder.HasMany(p => p.PollResults)
			.WithOne(p => p.TwitchStream)
			.HasForeignKey(p => p.TwitchStreamId)
			.IsRequired();

		//builder.HasMany(p => p.StreamStats)
		//	.WithOne(p => p.TwitchStream)
		//	.HasForeignKey(p => p.TwitchStreamId)
		//	.IsRequired();

		builder.HasOne(p => p.OutgoingRaid)
			.WithOne(p => p.TwitchStream)
			.HasForeignKey<OutgoingRaid>(p => p.TwitchStreamId)
			.IsRequired();

		builder.HasOne(p => p.DandleResult)
			.WithOne(p => p.TwitchStream)
			.HasForeignKey<DandleResult>(p => p.TwitchStreamId)
			.IsRequired();
	}
}