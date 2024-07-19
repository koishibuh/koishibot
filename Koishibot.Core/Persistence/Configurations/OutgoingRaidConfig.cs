using Koishibot.Core.Features.RaidSuggestions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class OutgoingRaidConfig
	 : IEntityTypeConfiguration<OutgoingRaid>
{
	public void Configure(EntityTypeBuilder<OutgoingRaid> builder)
	{
		builder.ToTable("OutgoingRaids");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchStreamId);

		builder.Property(p => p.RaidedUserId);

		builder.Property(p => p.SuggestedByUserId);

		builder.HasOne(p => p.SuggestedByUser)
			.WithMany(p => p.RaidsSuggestedByThisUser)
			.HasForeignKey(p => p.SuggestedByUserId)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.HasOne(p => p.RaidedUser)
			.WithMany(p => p.UsersSuggestingThisRaidTarget)
			.HasForeignKey(p => p.RaidedUserId)
			.OnDelete(DeleteBehavior.ClientSetNull);
	}
}