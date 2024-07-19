namespace Koishibot.Core.Features.Common;

public record SendAnnouncementApi(ITwitchAPI TwitchApi,
		IOptions<Settings> Settings, IRefreshAccessTokenService TokenProcessor,
		ILogger<SendAnnouncementApi> Log)
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;


	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-announcement">Send Announcement Documentation</see>
	/// Case-sensitive Colors: blue, green, orange, purple, primary (default)
	/// </summary>
	/// <returns></returns>
	public async Task SendAnnouncement(string message)
	{
		await TokenProcessor.EnsureValidToken();

		await TwitchApi.Helix.Chat.SendChatAnnouncementAsync(StreamerId, StreamerId, message);
	}
}