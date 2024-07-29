using Koishibot.Core.Features.ChatCommands.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;

public record TimerGroupConfig()
{
	public void Configure(EntityTypeBuilder<TimerGroup> builder)
	{
		builder.ToTable("TimerGroups");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder
			.HasIndex(p => p.Name)
			.IsUnique();
		
		builder.Property(p => p.Name);

		builder.Property(p => p.Description);
		builder.Property(p => p.Interval);
	}
}