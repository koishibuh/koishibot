using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Kofi;
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
	public string KofiType { get; set; }
	public int? UserId { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string Currency { get; set; } = string.Empty;
	public int AmountInPence { get; set; }

	// NAVIGATION

	public TwitchUser? TwitchUser { get; set; }

	public StreamEventVm CreateVm(string amount)
	{
		return new StreamEventVm()
		{
			EventType = StreamEventType.Kofi,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{Username} tipped {amount} via Kofi",
			Amount = AmountInPence
		};
	}

	public Kofi CreateKofi(KofiEvent data, int? userId)
	{
		KofiTransactionId = data.KofiTransactionId;
		Timestamp = data.Timestamp;
		TransactionUrl = data.Url;
		KofiType = data.Type.ToString();
		UserId = userId;
		Username = data.FromName;
		Message = data.Message ?? string.Empty;
		Currency = data.Currency;
		AmountInPence = Toolbox.AmountStringToPence(data.Amount);
		return this;
	}
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
		
		builder.HasIndex(p => p.Timestamp);
		builder.Property(p => p.Timestamp);
		
		builder.Property(p => p.TransactionUrl);
		builder.Property(p => p.KofiType);
		builder.Property(p => p.UserId);
		builder.Property(p => p.Username);
		builder.Property(p => p.Message);
		builder.Property(p => p.Currency);
		builder.Property(p => p.AmountInPence);
	}
}