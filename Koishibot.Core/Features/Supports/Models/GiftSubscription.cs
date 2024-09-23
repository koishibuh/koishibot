using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class GiftSubscription : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }
	public string Tier { get; set; } = string.Empty;
	public int Total { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public GiftSubscription Initialize(int userId, string tier, int total)
	{
		Timestamp = DateTimeOffset.UtcNow;
		UserId = userId;
		Tier = tier;
		Total = total;
		return this;
	}

}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class GiftSubscriptionConfig : IEntityTypeConfiguration<GiftSubscription>
{
	public void Configure(EntityTypeBuilder<GiftSubscription> builder)
	{
		builder.ToTable("GiftSubscriptions");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.Tier);

		builder.Property(p => p.Total);
	}
}