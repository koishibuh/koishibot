using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Obs.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ObsItem
{
	public int Id { get; set; }
	public string Type { get; set; } = string.Empty;
	public string ObsName { get; set; } = string.Empty;
	public string ObsId { get; set; } = string.Empty;
	public string? AppName { get; set; }
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

/*══════════════════【 CONFIGURATION 】═════════════════*/
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