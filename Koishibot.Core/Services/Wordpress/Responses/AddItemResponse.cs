namespace Koishibot.Core.Services.Wordpress.Responses;

public class AddItemResponse
{
	[JsonPropertyName("id")]
	public int WordpressId { get; set; }

	[JsonPropertyName("date")]
	public DateTimeOffset Date {get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("title")]
	public Title Title { get; set; }
}


public class Title
{
	[JsonPropertyName("raw")]
	public string Raw { get; set; }
}