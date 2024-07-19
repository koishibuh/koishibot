using Koishibot.Core.Features.AttendanceLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Persistence.Configurations;

public class AttendanceConfig : IEntityTypeConfiguration<Attendance>
{
	public void Configure(EntityTypeBuilder<Attendance> builder)
	{
		builder.ToTable("Attendances");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.UserId);

		builder.Property(p => p.AttendanceCount);

		builder.Property(p => p.StreakCurrentCount);

		builder.Property(p => p.StreakPersonalBest);

		builder.Property(p => p.StreakOptOut);
	}
}
