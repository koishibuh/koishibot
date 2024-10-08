using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Raids.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class IncomingRaid : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int RaidedByUserId { get; set; }
	public int ViewerCount { get; set; }
	public TwitchUser RaidedByUser { get; set; } = null!;

	public StreamEventVm CreateVm() => new()
	{
		EventType = StreamEventType.Raid,
		Timestamp = Toolbox.CreateUITimestamp(),
		Message = $"{RaidedByUser.Name} has raided with {ViewerCount}"
	};
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class IncomingRaidConfig : IEntityTypeConfiguration<IncomingRaid>
{
	public void Configure(EntityTypeBuilder<IncomingRaid> builder)
	{
		builder.ToTable("IncomingRaids");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.RaidedByUserId);

		builder.Property(p => p.ViewerCount);

		builder.HasOne(p => p.RaidedByUser)
			.WithMany(p => p.RaidsFromThisUser)
			.HasForeignKey(p => p.RaidedByUserId)
			.IsRequired();
	}
}