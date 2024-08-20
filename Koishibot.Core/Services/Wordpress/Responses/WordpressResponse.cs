namespace Koishibot.Core.Services.Wordpress.Responses;


public class WordPressResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("count")]
	public int Count { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("taxonomy")]
	public string Taxonomy { get; set; }
}