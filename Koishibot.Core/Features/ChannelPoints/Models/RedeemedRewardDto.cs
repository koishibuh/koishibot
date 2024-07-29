using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public record RedeemedRewardDto(
	TwitchUserDto User,
	string? Message,
	DateTimeOffset RedeemedAt,
	string? Title,
	string? RewardId,
	string? Description,
	string FullfillmentStatus,
	int? Cost
);
