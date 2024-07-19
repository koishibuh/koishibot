using Koishibot.Core.Features.Supports.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Persistence.Configurations;


public class ChannelFollowConfig : IEntityTypeConfiguration<ChannelFollow>
{
	public void Configure(EntityTypeBuilder<ChannelFollow> builder)
	{
		builder.ToTable("ChannelFollow");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);
	}
}
