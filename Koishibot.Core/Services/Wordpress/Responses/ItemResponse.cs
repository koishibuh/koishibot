using Koishibot.Core.Persistence;
namespace Koishibot.Core.Services.Wordpress.Responses;

public class ItemResponse
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

	[JsonPropertyName("content")]
	public Content Content { get; set; }

	[JsonPropertyName("item_tag")]
	public List<int> ItemTagIds { get; set; }
}


public class Title
{
	[JsonPropertyName("raw")]
	public string Raw { get; set; }

	[JsonPropertyName("rendered")]
	public string Rendered { get; set; }
}


public class Content
{
	[JsonPropertyName("rendered")]
	public string Rendered { get; set; }
}

public static class ItemResponseExtensions
{
	public static bool DragonRecorded(this KoishibotDbContext database, ItemResponse item) =>
		database.KoiKinDragons.Any(x => x.WordpressId == item.WordpressId);
}