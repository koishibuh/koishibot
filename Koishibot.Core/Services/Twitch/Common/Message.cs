namespace Koishibot.Core.Services.Twitch.Common;

public class Message
{
	///<summary>
	///The chat message in plain text.
	///</summary>
	[JsonPropertyName("text")]
	public string? Text { get; set; }

	///<summary>
	///Ordered list of chat message fragments.
	///</summary>
	[JsonPropertyName("fragments")]
	public List<Fragments>? Fragments { get; set; } 
}
