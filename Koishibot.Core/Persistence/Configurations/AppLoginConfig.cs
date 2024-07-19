using Koishibot.Core.Features.ApplicationAuthentication.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;


public class AppLoginConfig
	 : IEntityTypeConfiguration<AppLogin>
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