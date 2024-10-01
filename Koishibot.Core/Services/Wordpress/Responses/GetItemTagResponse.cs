namespace Koishibot.Core.Services.Wordpress.Responses;

public class GetItemTagResponse
{
	[JsonPropertyName("id")]
	public int WordpressId { get; set; }

	[JsonPropertyName("name")]
	public string Name {get; set; }
}