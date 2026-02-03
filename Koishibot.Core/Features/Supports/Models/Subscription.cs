using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class Subscription : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }	
	public int UserId { get; set; }
	public string Type { get; set; } = string.Empty;
	public string? Message { get; set; }
	public string? EventMessage { get; set; }
	public string? UserMessage { get; set; }
	

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;
}

// Sub, Resub, Gifted Sub, Prime sub

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
{
	public void Configure(EntityTypeBuilder<Subscription> builder)
	{
		builder.ToTable("Subscriptions");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.HasIndex(p => p.Timestamp);
		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.Type);

		builder.Property(p => p.Message);
		builder.Property(p => p.EventMessage);
		builder.Property(p => p.UserMessage);
	}
}