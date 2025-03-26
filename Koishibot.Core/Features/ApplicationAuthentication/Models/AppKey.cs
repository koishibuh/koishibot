using Koishibot.Core.Features.ChatCommands.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Features.ApplicationAuthentication.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class AppKey : IEntity
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Key { get; set; }
	public string Value { get; set; }

}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class AppKeyConfig : IEntityTypeConfiguration<AppKey>
{
	public void Configure(EntityTypeBuilder<AppKey> builder)
	{
		builder.ToTable("AppKeys");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.Name);

		builder.Property(p => p.Key);

		builder.Property(p => p.Value);
	}
}