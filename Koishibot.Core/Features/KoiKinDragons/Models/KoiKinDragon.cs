using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Features.KoiKinDragons.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class KoiKinDragon : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public string Code { get; set; }
	public string Name { get; set; }
	public int UserId { get; set; }
	public TwitchUser TwitchUser { get; set; } = null!;
}

public record KoiKinDragonVm(int dragonId, string code, string username);

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class KoiKinDragonExtensions
{
	public static async Task<KoiKinDragonVm?> GetLastUnnamedAdultDragon(this KoishibotDbContext database)
	{
		return await database.KoiKinDragons
			.Where(x => x.Name == "?" && x.Timestamp <= DateTimeOffset.Now.AddDays(-14))
			.OrderBy(x => x.Timestamp)
			.Include(x => x.TwitchUser)
			.Select(x => new KoiKinDragonVm(x.Id, x.Code, x.TwitchUser.Name ?? ""))
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
		
		builder.Property(p => p.Timestamp);

		builder.Property(p => p.Code);
		builder.HasIndex(p => p.Code)
			.IsUnique();

		builder.Property(p => p.Name);
	}
}