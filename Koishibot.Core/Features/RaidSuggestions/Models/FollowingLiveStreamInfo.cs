using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.RaidSuggestions.Models;

public record FollowingLiveStreamInfo(
  TwitchUserDto User,
  LiveStreamInfo LiveStreamInfo
  );