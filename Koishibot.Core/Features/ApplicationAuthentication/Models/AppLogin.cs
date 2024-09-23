using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.ApplicationAuthentication.Models;


/*═════════════════【 ENTITY MODEL 】═════════════════*/
public record AppLogin(
	int Id,
	string Username,
	string HashedPassword
	);

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class AppLoginConfig : IEntityTypeConfiguration<AppLogin>
{
	public void Configure(EntityTypeBuilder<AppLogin> builder)
	{
		builder.ToTable("AppLogin");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.Username);

		builder.Property(p => p.HashedPassword);
	}
}