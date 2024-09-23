using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class WordpressItemTag : IEntity
{
	public int Id { get; set; }
	public int WordPressId { get; set; }

	// NAVIGATION

	public int? UserId { get; set; }
	public TwitchUser? TwitchUser { get; set; }

	public List<KoiKinDragon> KoiKinDragons { get; set; } = [];
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ItemTagConfig : IEntityTypeConfiguration<WordpressItemTag>
{
	public void Configure(EntityTypeBuilder<WordpressItemTag> builder)
	{
		builder.ToTable("WordpressItemTags");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.UserId);
		builder.Property(p => p.WordPressId);
		builder.Property(p => p.UserId);

		builder.HasMany(p => p.KoiKinDragons)
			.WithOne(p => p.ItemTag)
			.HasForeignKey(p => p.ItemTagId)
			.IsRequired(false);
	}
}