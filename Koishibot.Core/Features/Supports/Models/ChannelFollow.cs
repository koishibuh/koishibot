using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChannelFollow(int id) : IEntity
{

	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
	public int UserId { get; set; } = id;

	public TwitchUser TwitchUser { get; set; } = null!;
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
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