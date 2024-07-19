using Koishibot.Core.Features.Raids.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class IncomingRaidConfig
	 : IEntityTypeConfiguration<IncomingRaid>
{
	public void Configure(EntityTypeBuilder<IncomingRaid> builder)
	{
		builder.ToTable("IncomingRaids");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchStreamId);

		builder.Property(p => p.RaidedAt);

		builder.Property(p => p.RaidedByUserId);

		builder.Property(p => p.ViewerCount);

		builder.HasOne(p => p.RaidedByUser)
			.WithMany(p => p.RaidsFromThisUser)
			.HasForeignKey(p => p.RaidedByUserId)
			.IsRequired();

		builder.HasOne(p => p.TwitchStream)
			.WithMany(p => p.IncomingRaids)
			.HasForeignKey(p => p.TwitchStreamId)
			.IsRequired();
	}
}