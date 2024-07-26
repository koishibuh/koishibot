using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-cheermotes">Twitch Documentation</see><br/>
	/// Gets a list of Cheermotes that users can use to cheer Bits in any Bits-enabled channel’s chat room. Cheermotes are animated emotes that viewers can assign Bits to.<br/>
	/// Required Scopes: User Access Token <br/>
	/// </summary>
	public async Task GetCheermotes(GetCheermotesRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "bits/cheermotes";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetCheermotesRequestParameters
{
	///<summary>
	/// The ID of the broadcaster whose custom Cheermotes you want to get.<br/>
	/// Specify the broadcaster’s ID if you want to include the broadcaster’s Cheermotes in the response (not all broadcasters upload Cheermotes).<br/>
	/// If not specified, the response contains only global Cheermotes.<br/>
	/// If the broadcaster uploaded Cheermotes, the type field in the response is set to channel_custom.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

// == ⚫ RESPONSE BODY == //

public class GetCheermotesResponse
{
	///<summary>
	///The name portion of the Cheermote string that you use in chat to cheer Bits.<br/>
	///The full Cheermote string is the concatenation of {prefix} + {number of Bits}.<br/>
	///For example, if the prefix is “Cheer” and you want to cheer 100 Bits, the full Cheermote string is Cheer100.<br/>
	///When the Cheermote string is entered in chat, Twitch converts it to the image associated with the Bits tier that was cheered.
	///</summary>
	[JsonPropertyName("prefix")]
	public string Prefix { get; set; }

	///<summary>
	///A list of tier levels that the Cheermote supports.<br/>
	///Each tier identifies the range of Bits that you can cheer at that tier level and an image that graphically identifies the tier level.
	///</summary>
	[JsonPropertyName("tiers")]
	public List<CheermoteTier> CheermoteTiers { get; set; }

	///<summary>
	///The type of Cheermote
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(CheermoteTypeEnumConverter))]
	public CheermoteType Type { get; set; }

	///<summary>
	///The order that the Cheermotes are shown in the Bits card.<br/>
	///The numbers may not be consecutive - for example, the numbers may jump from 1 to 7 to 13.<br/>
	///The order numbers are unique within a Cheermote type (for example, global_first_party) but may not be unique amongst all Cheermotes in the response.
	///</summary>
	[JsonPropertyName("order")]
	public int Order { get; set; }

	///<summary>
	///The timestamp when this Cheermote was last updated.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("last_updated")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public DateTimeOffset LastUpdated { get; set; }

	///<summary>
	///A Boolean value that indicates whether this Cheermote provides a charitable contribution match during charity campaigns.
	///</summary>
	[JsonPropertyName("is_charitable")]
	public bool IsCharitable { get; set; }
}

public class CheermoteTier
{
	///<summary>
	///The minimum number of Bits that you must cheer at this tier level.<br/>
	///The maximum number of Bits that you can cheer at this level is determined by the required minimum Bits of the next tier level minus 1.<br/>
	///For example, if min_bits is 1 and min_bits for the next tier is 100, the Bits range for this tier level is 1 through 99.<br/>
	///The minimum Bits value of the last tier is the maximum number of Bits you can cheer using this Cheermote. For example, 10000.
	///</summary>
	[JsonPropertyName("min_bits")]
	public int MinBits { get; set; }

	///<summary>
	///The tier level.
	///</summary>
	[JsonPropertyName("id")]
	[JsonConverter(typeof(CheermoteTierLevelEnumConverter))]
	public CheermoteTierLevel CheermoteTierLevel { get; set; }

	///<summary>
	///The hex code of the color associated with this tier level (for example, #979797).
	///</summary>
	[JsonPropertyName("color")]
	public string Color { get; set; }

	///<summary>
	///The animated and static image sets for the Cheermote.<br/>
	///The dictionary of images is organized by theme, format, and size.<br/>
	///The theme keys are dark and light.<br/>
	///Each theme is a dictionary of formats: animated and static.<br/>
	///Each format is a dictionary of sizes: 1, 1.5, 2, 3, and 4.<br/>
	///The value of each size contains the URL to the image.
	///</summary>
	[JsonPropertyName("images")]
	public Dictionary<string, string> Images { get; set; }

	///<summary>
	///A Boolean value that determines whether users can cheer at this tier level.
	///</summary>
	[JsonPropertyName("can_cheer")]
	public bool CanCheer { get; set; }

	///<summary>
	///A Boolean value that determines whether this tier level is shown in the Bits card.<br/>
	///Is true if this tier level is shown in the Bits card.
	///</summary>
	[JsonPropertyName("show_in_bits_card")]
	public bool ShowInBitsCard { get; set; }
}