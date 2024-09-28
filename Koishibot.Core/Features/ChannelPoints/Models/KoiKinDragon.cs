using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ChannelPoints.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class KoiKinDragon : IEntity
{
	public int Id { get; set; }
	public int WordpressId { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public string Code { get; set; }
	public string Name { get; set; }

	public int? ItemTagId { get; set; }
	public WordpressItemTag? ItemTag { get; set; }
}

public record KoiKinDragonVm(int wordpressId, string code, string username);

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class KoiKinDragonExtensions
{
	public static async Task<KoiKinDragonVm> GetLastUnnamedAdultDragon(this KoishibotDbContext database)
	{
		return await database.KoiKinDragons
			.Where(x => x.Name == "?" && x.Timestamp <= DateTimeOffset.Now.AddDays(-14))
			.OrderBy(x => x.Timestamp)
			.Include(x => x.ItemTag)
			.ThenInclude(x => x.TwitchUser)
			.Select(x => new KoiKinDragonVm(x.WordpressId, x.Code, x.ItemTag.TwitchUser.Name ?? "" ) )
			.AsNoTracking()
			.FirstOrDefaultAsync();
	}
}


/*══════════════════【 CONFIGURATION 】═════════════════*/
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