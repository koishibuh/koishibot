using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public class ItemTag
{
	public int Id { get; set; }
	public int WordPressId { get; set; }
	public int? UserId { get; set; }

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
	public List<KoiKinDragon> KoiKinDragons { get; set; } = [];
}

public class ItemTagConfig : IEntityTypeConfiguration<ItemTag>
{
	public void Configure(EntityTypeBuilder<ItemTag> builder)
	{
		builder.ToTable("ItemTags");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.UserId);
		builder.Property(p => p.WordPressId);

		builder.HasMany(p => p.KoiKinDragons)
			.WithOne(p => p.ItemTag)
			.HasForeignKey(p => p.ItemTagId)
			.IsRequired(false);
	}
}