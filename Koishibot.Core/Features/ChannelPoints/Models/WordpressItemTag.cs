using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class WordpressItemTag(int? userId, int wordPressId) : IEntity
{
	public int Id { get; set; }
	public int WordPressId { get; set; } = wordPressId;

	// NAVIGATION

	public int? UserId { get; set; } = userId;
	public TwitchUser? TwitchUser { get; set; }

	public List<KoiKinDragon> KoiKinDragons { get; set; } = [];
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class WordpressItemTagExtensions
{
	public static async Task<WordpressItemTag?> GetItemTagByUserId
		(this KoishibotDbContext database, int userId)
	{
		return await database.WordpressItemTags
			.Where(x => x.UserId == userId)
			.FirstOrDefaultAsync();
	}

	public static async Task<WordpressItemTag?> GetItemTagByWordpressId
		(this KoishibotDbContext database, int wordpressId)
	{
		return await database.WordpressItemTags
			.Where(x => x.WordPressId == wordpressId)
			.FirstOrDefaultAsync();
	}
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