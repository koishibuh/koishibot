using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#block-user">Twitch Documentation</see><br/>
	/// Blocks the specified user from interacting with or having contact with the broadcaster.<br/>
	/// The user ID in the OAuth token identifies the broadcaster who is blocking the user.<br/>
	/// </summary>
	public async Task BlockUser(BlockUserRequestParameters parameters)
	{
		var method = HttpMethod.Put;
		var url = "users/blocks";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class BlockUserRequestParameters
{
	///<summary>
	///The ID of the user to block.<br/>
	///The API ignores the request if the broadcaster has already blocked the user.
	///</summary>
	[JsonPropertyName("target_user_id")]
	public string TargetUserId { get; set; }

	///<summary>
	///The location where the harassment took place that is causing the brodcaster to block the user.
	///</summary>
	[JsonPropertyName("source_context")]
	[JsonConverter(typeof(SourceContext))]
	public SourceContext SourceContext { get; set; }

	///<summary>
	///The reason that the broadcaster is blocking the user.
	///</summary>
	[JsonPropertyName("reason")]
	[JsonConverter(typeof(BanReason))]
	public BanReason Reason { get; set; }

}