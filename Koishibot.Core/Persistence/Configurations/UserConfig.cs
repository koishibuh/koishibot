using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;

public class UserConfig : IEntityTypeConfiguration<TwitchUser>
{
	public void Configure(EntityTypeBuilder<TwitchUser> builder)
	{
		builder.ToTable("Users");
		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.TwitchId);

		builder.HasIndex(p => p.TwitchId)
			.IsUnique();

		builder.Property(p => p.Login);

		builder.Property(p => p.Name);

		builder.Property(p => p.Permissions);

		builder.HasOne(p => p.Attendance)
			.WithOne(p => p.User)
			.HasForeignKey<Attendance>(p => p.UserId);

		builder.HasMany(p => p.ChannelFollows)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey(p => p.UserId)
			.IsRequired();

		builder.HasMany(p => p.Cheers)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey(p => p.UserId)
			.IsRequired();

		builder.HasMany(p => p.Subscriptions)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey(p => p.UserId)
			.IsRequired();

		builder.HasMany(p => p.GiftedSubscriptions)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey(p => p.UserId)
			.IsRequired();

		builder.HasOne(p => p.SupportTotal)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey<SupportTotal>(p => p.UserId);

		builder.HasMany(p => p.KofiSupport)
		.WithOne(p => p.TwitchUser)
		.HasForeignKey(p => p.UserId)
		.IsRequired(false);

		builder.HasMany(p => p.SETips)
			.WithOne(p => p.TwitchUser)
			.HasForeignKey(p => p.UserId)
			.IsRequired(false);
	}
}