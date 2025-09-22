using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class TwitchCheer : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public int UserId { get; set; }
	public int BitsAmount { get; set; }
	public string Message { get; set; } = string.Empty;
	
	// public int? StreamSessionId { get; set; } 
	// public StreamSession? StreamSession { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser TwitchUser { get; set; } = null!;

	// == ⚫ METHODS == //

	public TwitchCheer Initialize(int userId, int bits, string message)
	{
		Timestamp = DateTimeOffset.UtcNow;
		UserId = userId;
		BitsAmount = bits;
		Message = message;

		return this;
	}

	public async Task UpdateRepo(KoishibotDbContext context)
	{
		context.Add(this);
		await context.SaveChangesAsync();
	}

	public StreamEventVm CreateVm(string username)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Cheer,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{username} has cheered {BitsAmount}"
		};
	}

	public StreamEventVm CreateVm(string username, RewardType rewardType)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Cheer,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{username} has cheered {BitsAmount} bits by {rewardType}"
		};
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class TwitchCheerConfig : IEntityTypeConfiguration<TwitchCheer>
{
	public void Configure(EntityTypeBuilder<TwitchCheer> builder)
	{
		builder.ToTable("Cheers");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.Timestamp);

		builder.Property(p => p.UserId);

		builder.Property(p => p.BitsAmount);

		builder.Property(p => p.Message);
	}
}