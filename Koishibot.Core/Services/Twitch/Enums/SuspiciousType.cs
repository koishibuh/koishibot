using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


[JsonConverter(typeof(SuspiciousTypeEnumConverter))]
public enum SuspiciousType
{
	Manual = 1,
	BanEvaderDetector,
	SharedChannelBan
}

// == ⚫ == //

public class SuspiciousTypeEnumConverter : JsonConverter<SuspiciousType>
{
	public override SuspiciousType Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"manual" => SuspiciousType.Manual,
			"ban_evader_detector" => SuspiciousType.BanEvaderDetector,
			"shared_channel_ban" => SuspiciousType.SharedChannelBan,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, SuspiciousType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SuspiciousType.Manual => "manual",
			SuspiciousType.BanEvaderDetector => "ban_evader_detector",
			SuspiciousType.SharedChannelBan => "shared_channel_ban",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}