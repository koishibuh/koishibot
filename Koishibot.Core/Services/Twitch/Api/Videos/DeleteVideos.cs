using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ DELETE == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-videos">Twitch Documentation</see><br/>
	/// Deletes one or more videos. You may delete past broadcasts, highlights, or uploads<br/>
	/// Required Scopes:  channel:manage:videos
	/// </summary>
	public async Task DeleteVideos(DeleteVideosRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "videos";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class DeleteVideosRequestParameters
{
	///<summary>
	///The list of videos to delete.<br/>
	///To specify more than one video, include the id parameter for each video to delete.<br/>
	///For example, id=1234 and id=5678.<br/>
	///You can delete a maximum of 5 videos per request. Ignores invalid video IDs.<br/>
	///If the user doesn’t have permission to delete one of the videos in the list, none of the videos are deleted.<br/>
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class DeleteVideosResponse
{
	///<summary>
	///The list of IDs of the videos that were deleted
	///</summary>
	[JsonPropertyName("data")]
	public List<string> DeleteVideoIds { get; set; }
}