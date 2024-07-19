using Koishibot.Core.Features.Polls.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class PollConfig
	: IEntityTypeConfiguration<PollResult>
{
	public void Configure(EntityTypeBuilder<PollResult> builder)
	{
		builder.ToTable("Polls");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchStreamId);

		builder.Property(p => p.StartedAt);

		builder.Property(p => p.Title);

		builder.Property(p => p.ChoiceOne);

		builder.Property(p => p.VoteOne);

		builder.Property(p => p.ChoiceTwo);

		builder.Property(p => p.VoteTwo);

		builder.Property(p => p.ChoiceThree);

		builder.Property(p => p.VoteThree);

		builder.Property(p => p.ChoiceFour);

		builder.Property(p => p.VoteFour);

		builder.Property(p => p.ChoiceFive);

		builder.Property(p => p.VoteFive);

		builder.Property(p => p.WinningChoice);

		builder.HasOne(p => p.TwitchStream)
			.WithMany(p => p.PollResults)
			.HasForeignKey(p => p.TwitchStreamId);
	}
}
