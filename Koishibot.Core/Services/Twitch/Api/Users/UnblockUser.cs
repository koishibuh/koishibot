using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;


// == ⚫ DELETE == //
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#unblock-user">Twitch Documentation</see><br/>
	/// Removes the user from the broadcaster’s list of blocked users.<br/>
	/// The user ID in the OAuth token identifies the broadcaster who’s removing the block.<br/>
	/// Required Scopes: user:manage:blocked_users<br/>
	/// </summary>
	public async Task UnblockUser(UnblockUserRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "users/blocks";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UnblockUserRequestParameters
{
	///<summary>
	///The ID of the user to remove from the broadcaster’s list of blocked users.<br/>
	///The API ignores the request if the broadcaster hasn’t blocked the user.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("target_user_id")]
	public string TargetUserId { get; set; } = null!;
}


