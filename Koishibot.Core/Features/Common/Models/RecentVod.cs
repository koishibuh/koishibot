namespace Koishibot.Core.Features.Common.Models;

public record RecentVod(
    string TwitchUserId,
    string VideoId,
    string CreatedAt,
    string Duration,
    string PublishedAt,
    string Url,
    string Title,
    string Type,
    string Description,
    string ThumbnailUrl,
    string Viewable,
    int ViewCount
    );

