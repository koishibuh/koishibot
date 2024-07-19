using TwitchLib.Api.Core.Enums;

namespace Koishibot.Core.Services.TwitchEventSub;

public static class TwitchEventSubSubscriber
{
	public static async Task CreateEventSubBroadcaster
		(this ITwitchAPI TwitchApi, string eventname, string version, IOptions<Settings> Settings)
	{
		await TwitchApi.Helix.EventSub.CreateEventSubSubscriptionAsync(
			eventname, version,
			new Dictionary<string, string> 
				{{ "broadcaster_user_id", Settings.Value.StreamerTokens.UserId } },
			EventSubTransportMethod.Websocket,
			Settings.Value.TwitchEventSubSessionId);
	}

	public static async Task CreateEventSubFromBroadcaster
		(this ITwitchAPI TwitchApi, string eventname, string version, IOptions<Settings> Settings)
	{
		await TwitchApi.Helix.EventSub.CreateEventSubSubscriptionAsync(
			eventname, version,
			new Dictionary<string, string> 
				{{ "from_broadcaster_user_id", Settings.Value.StreamerTokens.UserId }},
			EventSubTransportMethod.Websocket,
			Settings.Value.TwitchEventSubSessionId);
	}

	public static async Task CreateEventSubToBroadcaster
		(this ITwitchAPI TwitchApi, string eventname, string version, IOptions<Settings> Settings)
	{
		await TwitchApi.Helix.EventSub.CreateEventSubSubscriptionAsync(
			eventname, version,
			new Dictionary<string, string> 
				{{ "to_broadcaster_user_id", Settings.Value.StreamerTokens.UserId }},
			EventSubTransportMethod.Websocket,
			Settings.Value.TwitchEventSubSessionId);
	}

	public static async Task CreateEventSubMod
		(this ITwitchAPI TwitchApi, string eventname, string version, IOptions<Settings> Settings)
	{
		await TwitchApi.Helix.EventSub.CreateEventSubSubscriptionAsync(
			eventname, version,
			new Dictionary<string, string>
			{
				{ "broadcaster_user_id", Settings.Value.StreamerTokens.UserId },
				{ "moderator_user_id", Settings.Value.StreamerTokens.UserId } 
			},
			EventSubTransportMethod.Websocket,
			Settings.Value.TwitchEventSubSessionId);
	}
}
