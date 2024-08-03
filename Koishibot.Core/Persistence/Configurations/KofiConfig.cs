using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


public class KofiConfigConfig : IEntityTypeConfiguration<Kofi>
{
	public void Configure(EntityTypeBuilder<Kofi> builder)
	{
		builder.ToTable("Kofi");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.KofiTransactionId);
		builder.Property(p => p.Timestamp);
		builder.Property(p => p.TransactionUrl);
		builder.Property(p => p.KofiType);
		builder.Property(p => p.UserId);
		builder.Property(p => p.Username);
		builder.Property(p => p.Message);
		builder.Property(p => p.Currency);
		builder.Property(p => p.Amount);
	}
}