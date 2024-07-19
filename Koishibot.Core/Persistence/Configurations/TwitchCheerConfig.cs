using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;
public class TwitchCheerConfig : IEntityTypeConfiguration<TwitchCheer>
{
	public void Configure(EntityTypeBuilder<TwitchCheer> builder)
	{
		builder.ToTable("Cheers");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);
		
		builder.Property(p => p.BitsAmount);
		
		builder.Property(p => p.Message);
	}
}