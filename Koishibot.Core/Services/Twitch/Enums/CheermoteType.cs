using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

//global_first_party — A Twitch-defined Cheermote that is shown in the Bits card.
//global_third_party — A Twitch-defined Cheermote that is not shown in the Bits card.
//channel_custom — A broadcaster-defined Cheermote.
//display_only — Do not use; for internal use only.
//sponsored — A sponsor-defined Cheermote. When used, the sponsor adds additional Bits to the amount that the user cheered. For example, if the user cheered Terminator100, the broadcaster might receive 110 Bits, which includes the sponsor's 10 Bits contribution.

[JsonConverter(typeof(CheermoteTypeEnumConverter))]
public enum CheermoteType
{
	GlobalFirstParty = 1,
	GlobalThirdParty,
	ChannelCustom,
	DisplayOnly,
	Sponsored
}

// == ⚫ == //

public class CheermoteTypeEnumConverter : JsonConverter<CheermoteType>
{
	public override CheermoteType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"global_first_party" => CheermoteType.GlobalFirstParty,
			"global_third_party" => CheermoteType.GlobalThirdParty,
			"channel_custom" => CheermoteType.ChannelCustom,
			"display_only" => CheermoteType.DisplayOnly,
			"sponsored" => CheermoteType.Sponsored,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, CheermoteType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			CheermoteType.GlobalFirstParty => "global_first_party",
			CheermoteType.GlobalThirdParty => "global_third_party",
			CheermoteType.ChannelCustom => "channel_custom",
			CheermoteType.DisplayOnly => "display_only",
			CheermoteType.Sponsored => "sponsored",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}