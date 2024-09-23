using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Dandle.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class DandleWord : IEntity
{
	public int Id { get; set; }
	public string Word { get; set; } = null!;

	public List<DandleResult> DandleResults { get; set; } = [];

	public DandleWord Set(string word)
	{
		Word = word;
		return this;
	}

	public DandleWord Set(int id)
	{
		Id = id;
		return this;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class DandleWordConfig : IEntityTypeConfiguration<DandleWord>
{
	public void Configure(EntityTypeBuilder<DandleWord> builder)
	{
		builder.ToTable("DandleWords");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.HasIndex(p => p.Word)
		.IsUnique();

		builder.Property(p => p.Word)
		.HasMaxLength(5);

		builder.HasMany(p => p.DandleResults)
		.WithOne(p => p.DandleWord)
		.HasForeignKey(p => p.DandleWordId);
	}
}