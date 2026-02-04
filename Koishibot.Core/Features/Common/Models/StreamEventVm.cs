using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;

namespace Koishibot.Core.Features.Common.Models;

public class StreamEventVm
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public StreamEventType EventType { get; set; }
	public string Timestamp { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public int? Amount { get; set; }

	public StreamEventVm CreateFollowEvent(string username)
	{
		EventType = StreamEventType.Follow;
		Timestamp = CurrentTime();
		Message = $"{username} has followed";
		return this;
	}

	public StreamEventVm CreateCheerEvent(BitsUsedEvent args)
	{
		EventType = StreamEventType.Cheer;
		Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm");
		Message = args.PowerUpData is not null
			? $"{args.CheererName} has cheered {args.BitAmount} with {args.PowerUpData.Type}"
			: $"{args.CheererName} has cheered {args.BitAmount}";
		Amount = args.BitAmount;
		return this;
	}

	public StreamEventVm CreateKofiEvent(string message, int amount)
	{
		EventType = StreamEventType.Kofi;
		Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm");
		Message = message;
		Amount = amount;
		return this;
	}

public string CurrentTime() => 
		(DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm");
};
