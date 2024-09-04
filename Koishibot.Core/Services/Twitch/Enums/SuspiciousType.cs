using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


// [JsonConverter(typeof(SuspiciousTypeEnumConverter))]
// public class SuspiciousType
// {
// 	public const string Manual = 1,
// 	public const string BanEvaderDetector,
// 	public const string SharedChannelBan
// }

// == ⚫ == //

public class SuspiciousTypeEnumConverter : JsonConverter<string>
{
	public override string Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"manual" => "Manual",
			"ban_evader_detector" => "BanEvaderDetector",
			"shared_channel_ban" => "SharedChannelBan",
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			"Manual" => "manual",
			"BanEvaderDetector" => "ban_evader_detector",
			"SharedChannelBan" => "shared_channel_ban",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}