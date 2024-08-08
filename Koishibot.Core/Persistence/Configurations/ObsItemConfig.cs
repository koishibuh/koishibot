using Koishibot.Core.Features.Obs.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class ObsItemConfig: IEntityTypeConfiguration<ObsItem>
{
	public void Configure(EntityTypeBuilder<ObsItem> builder)
	{
		builder.ToTable("ObsItems");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Type);

		builder.Property(p => p.ObsName);
		builder.Property(p => p.ObsId);

		builder.Property(p => p.AppName);
	}
}
