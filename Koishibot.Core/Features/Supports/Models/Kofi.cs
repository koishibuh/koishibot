using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Kofi.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.Supports.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class Kofi : IEntity
{
	public int Id { get; set; }
	public string KofiTransactionId { get; set; } = string.Empty;
	public DateTimeOffset Timestamp { get; set; }
	public string TransactionUrl { get; set; } = string.Empty;	
	public KofiType KofiType { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Currency { get; set; } = string.Empty;
	public string Amount { get; set; } = string.Empty;	

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class KofiConfigConfig : IEntityTypeConfiguration<Kofi>
{
	public void Configure(EntityTypeBuilder<Kofi> builder)
	{
		builder.ToTable("Kofi");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.KofiTransactionId);
		builder.Property(p => p.Timestamp);
		builder.Property(p => p.TransactionUrl);
		builder.Property(p => p.KofiType);
		builder.Property(p => p.UserId);
		builder.Property(p => p.Username);
		builder.Property(p => p.Message);
		builder.Property(p => p.Currency);
		builder.Property(p => p.Amount);
	}
}