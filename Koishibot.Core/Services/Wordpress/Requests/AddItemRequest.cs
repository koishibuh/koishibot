namespace Koishibot.Core.Services.Wordpress.Requests;

public class AddItemRequest
{
	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("title")]
	public string Title { get; set; }

	[JsonPropertyName("content")]
	public string Content { get; set; }

	[JsonPropertyName("item_category")]
	public int ItemCategory { get; set; }

	[JsonPropertyName("item_tag")]
	public int ItemTag { get; set; }
}