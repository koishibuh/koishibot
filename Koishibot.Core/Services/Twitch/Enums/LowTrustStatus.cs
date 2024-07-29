using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(LowTrustStatusEnumConverter))]
public enum LowTrustStatus
{
	None = 1,
	ActiveMonitoring,
	Restricted
}

// == ⚫ == //

public class LowTrustStatusEnumConverter : JsonConverter<LowTrustStatus>
{
	public override LowTrustStatus Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"none" => LowTrustStatus.None,
			"active_monitoring" => LowTrustStatus.ActiveMonitoring,
			"restricted" => LowTrustStatus.Restricted,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, LowTrustStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			LowTrustStatus.None => "none",
			LowTrustStatus.ActiveMonitoring => "active_monitoring",
			LowTrustStatus.Restricted => "restricted",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}