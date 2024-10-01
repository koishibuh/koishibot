using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Polls.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class PollResult : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset StartedAt { get; set; }
	public string Title { get; set; } = null!;
	public string ChoiceOne { get; set; } = null!;
	public int VoteOne { get; set; }
	public string ChoiceTwo { get; set; } = null!;
	public int VoteTwo { get; set; }
	public string? ChoiceThree { get; set; }
	public int? VoteThree { get; set; }
	public string? ChoiceFour { get; set; }
	public int? VoteFour { get; set; }
	public string? ChoiceFive { get; set; }
	public int? VoteFive { get; set; }
	public string WinningChoice { get; set; } = null!;
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class PollConfig : IEntityTypeConfiguration<PollResult>
{
	public void Configure(EntityTypeBuilder<PollResult> builder)
	{
		builder.ToTable("Polls");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

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
	}
}