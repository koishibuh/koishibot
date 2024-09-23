using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class SETip : IEntity
{
	public int Id { get; set; }
	public string StreamElementsId { get; set; } = string.Empty;
	public DateTimeOffset Timestamp { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Amount { get; set; } = string.Empty;

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class SETipConfigConfig : IEntityTypeConfiguration<SETip>
{
	public void Configure(EntityTypeBuilder<SETip> builder)
	{
		builder.ToTable("SETips");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StreamElementsId);
		builder.Property(p => p.Timestamp);
		builder.Property(p => p.UserId);
		builder.Property(p => p.Username);
		builder.Property(p => p.Message);
		builder.Property(p => p.Amount);
	}
}