using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.StreamInformation.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class YearlyQuarter
{
	public int Id { get; set; }
	public DateOnly StartDate { get; set; }
	public DateOnly EndDate { get; set; }

	// Navigation

	// public IList<TwitchStream> TwitchStreams { get; set; } = [];
	public List<StreamSession> StreamSessions { get; set; } = [];

	// == ⚫ == //

	public YearlyQuarter Initialize()
	{
		var today = DateTime.UtcNow;

		int quarter = (today.Month - 1) / 3 + 1;
		int year = today.Year;
		int month = quarter * 3;

		if (month > 12)
		{
			month = 3;
			year++;
		}

		var endDate = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

		StartDate = DateOnly.FromDateTime(today);
		EndDate = endDate;

		return this;
	}

	public bool EndOfQuarter()
	{
		var today = DateTime.UtcNow;
		return DateOnly.FromDateTime(today) > EndDate;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class YearlyQuarterConfig : IEntityTypeConfiguration<YearlyQuarter>
{
	public void Configure(EntityTypeBuilder<YearlyQuarter> builder)
	{
		builder.ToTable("YearlyQuarters");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StartDate);

		builder.Property(p => p.EndDate);

		builder.HasMany(p => p.StreamSessions)
		.WithOne(p => p.YearlyQuarter)
		.HasForeignKey(p => p.YearlyQuarterId)
		.IsRequired();
	}
}