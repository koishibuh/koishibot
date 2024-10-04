using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Obs.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ObsItem : IEntity
{
	public int Id { get; set; }
	public string Type { get; set; } = string.Empty;
	public string ObsName { get; set; } = string.Empty;
	public string ObsId { get; set; } = string.Empty;
	public string? AppName { get; set; }

	public bool NameNotUpdated(ObsItem obsItem) => ObsName == obsItem.ObsName;
}

public class ObsItemType
{
	public const string Scene = "Scene";
	public const string Source = "Source";
	public const string Audio = "Audio";
}

public class ObsAppName
{
	public const string StartScene = "StartScene";
	public const string BRBScene = "BRBScene";
	public const string RaidScene = "RaidScene";
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class ObsItemExtensions
{
	public static async Task<ObsItem?> FindObsItemByObsId(this KoishibotDbContext database, string obsId) =>
		await database.ObsItems.FirstOrDefaultAsync(x => x.ObsId == obsId);
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ObsItemConfig : IEntityTypeConfiguration<ObsItem>
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