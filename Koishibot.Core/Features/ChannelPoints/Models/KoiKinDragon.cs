using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public class KoiKinDragon
{
	public int Id { get; set; }
	public int WordpressId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public string Code { get; set; }
	public string Name { get; set; }

	public int? ItemTagId { get; set; }
	public ItemTag? ItemTag { get; set; }
}

public class KoiKinDragonConfig : IEntityTypeConfiguration<KoiKinDragon>
{
	public void Configure(EntityTypeBuilder<KoiKinDragon> builder)
	{
		builder.ToTable("KoiKinDragons");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.WordpressId);
		builder.Property(p => p.Timestamp);
		builder.Property(p => p.Code);
		builder.Property(p => p.Name);
	}
}