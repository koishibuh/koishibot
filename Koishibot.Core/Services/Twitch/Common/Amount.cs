namespace Koishibot.Core.Services.Twitch.Common;

public class Amount
{
	///<summary>
	///The monetary amount.<br/>
	///The amount is specified in the currency’s minor unit.<br/>
	///For example, the minor units for USD is cents, so if the amount is $5.50 USD, value is set to 550.
	///</summary>
	[JsonPropertyName("value")]
	public int Value { get; set; }

	///<summary>
	///The number of decimal places used by the currency.<br/>
	///For example, USD uses two decimal places.
	///</summary>
	[JsonPropertyName("decimal_places")]
	public int DecimalPlaces { get; set; }

	///<summary>
	///The number of decimal places used by the currency.<br/>
	///For example, USD uses two decimal places.<br/>
	///In some documentation examples it has place without s?
	///</summary>
	[JsonPropertyName("decimal_place")]
	private int DecimalPlace { set { DecimalPlaces = value; } }

	///<summary>
	///The ISO-4217 three-letter currency code that identifies the type of currency in value.
	///</summary>
	[JsonPropertyName("currency")]
	public string? Currency { get; set; }
}