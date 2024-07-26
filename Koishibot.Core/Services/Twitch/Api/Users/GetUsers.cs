﻿using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-users">Twitch Documentation</see><br/>
	/// Gets information about one or more users.<br/>
	/// Lookup by UserId, UserLogin, or both but cannot exceed 100 users.
	/// If nothing is specified, request returns info about the user in the access token.
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetUsers(GetUsersRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "users";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetUsersRequestParameters
{
	///<summary>
	///The ID of the user to get.<br/>
	///To specify more than one user, include the id parameter for each user to get.<br/>
	///For example, id=123 id=5678. The maximum number of IDs you may specify is 100.
	///</summary>
	[JsonPropertyName("id")]
	public List<string>? UserIds { get; set; }

	///<summary>
	///The login name of the user to get. To specify more than one user, include the login parameter for each user to get. For example, login=foo&login=bar. The maximum number of login names you may specify is 100.
	///</summary>
	[JsonPropertyName("login")]
	public List<string>? UserLogins { get; set; }
}


// == ⚫ RESPONSE BODY == //

public class GetUsersResponse
{
	///<summary>
	///The list of users.
	///</summary>
	[JsonPropertyName("data")]
	public List<UserData> Users { get; set; }

}

public class UserData
{
	///<summary>
	///An ID that identifies the user.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("login")]
	public string Login { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("display_name")]
	public string DisplayName { get; set; }

	///<summary>
	///The type of user. 
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(UserTypeEnumConverter))]
	public UserType Type { get; set; }

	///<summary>
	///The type of broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_type")]
	[JsonConverter(typeof(BroadcasterTypeEnumConverter))]
	public BroadcasterType BroadcasterType { get; set; }

	///<summary>
	///The user’s description of their channel.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }

	///<summary>
	///A URL to the user’s profile image.
	///</summary>
	[JsonPropertyName("profile_image_url")]
	public string ProfileImageUrl { get; set; }

	///<summary>
	///A URL to the user’s offline image.
	///</summary>
	[JsonPropertyName("offline_image_url")]
	public string OfflineImageUrl { get; set; }

	/////<summary>
	/////The number of times the user’s channel has been viewed.<br/>
	/////NOTE: This field has been deprecated (see Get Users API endpoint – “view_count” deprecation). Any data in this field is not valid and should not be used.
	/////</summary>
	//[JsonPropertyName("view_count")]
	//public int ViewCount { get; set; }

	///<summary>
	///The user’s verified email address.<br/>
	///The object includes this field only if the user access token includes the user:read:email scope.<br/>
	///If the request contains more than one user, only the user associated with the access token that provided consent will include an email address — the email address for all other users will be empty.
	///</summary>
	[JsonPropertyName("email")]
	public string Email { get; set; }

	///<summary>
	///The timestamp that the user’s account was created.</br>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public string CreatedAt { get; set; }
}