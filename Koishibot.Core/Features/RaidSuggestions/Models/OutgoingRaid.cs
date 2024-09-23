using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.RaidSuggestions.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class OutgoingRaid
{
	public int Id { get; set; }
	// public int TwitchStreamId { get; set; }
	public int? StreamSessionId { get; set; }
	public int RaidedUserId { get; set; }
	public int SuggestedByUserId { get; set; }

	// NAVIGATION 

	// public TwitchStream TwitchStream { get; set; } = null!;
	public StreamSession StreamSession { get; set; }
	public TwitchUser RaidedUser { get; set; } = null!;
	public TwitchUser SuggestedByUser { get; set; } = null!;

	public OutgoingRaid Set(int? streamId, int raidedUserId, int suggestedByUserId)
	{
		StreamSessionId = streamId;
		// TwitchStreamId = streamId;
		RaidedUserId = raidedUserId;
		SuggestedByUserId = suggestedByUserId;
		return this;
	}

}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class OutgoingRaidConfig : IEntityTypeConfiguration<OutgoingRaid>
{
	public void Configure(EntityTypeBuilder<OutgoingRaid> builder)
	{
		builder.ToTable("OutgoingRaids");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.StreamSessionId);

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