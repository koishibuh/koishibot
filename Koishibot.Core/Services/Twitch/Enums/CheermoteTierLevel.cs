using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum CheermoteTierLevel
{
	Cheer1 = 1,
	Cheer100,
	Cheer500,
	Cheer1K,
	Cheer5K,
	Cheer10K,
	Cheer100K
}

// == ⚫ == //

public class CheermoteTierLevelEnumConverter : JsonConverter<CheermoteTierLevel>
{
	public override CheermoteTierLevel Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"1" => CheermoteTierLevel.Cheer1,
			"100" => CheermoteTierLevel.Cheer100,
			"500" => CheermoteTierLevel.Cheer500,
			"1000" => CheermoteTierLevel.Cheer1K,
			"5000" => CheermoteTierLevel.Cheer5K,
			"10000" => CheermoteTierLevel.Cheer10K,
			"100000" => CheermoteTierLevel.Cheer100K,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, CheermoteTierLevel value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			CheermoteTierLevel.Cheer1 => "1",
			CheermoteTierLevel.Cheer100 => "100",
			CheermoteTierLevel.Cheer500 => "500",
			CheermoteTierLevel.Cheer1K => "1000",
			CheermoteTierLevel.Cheer5K => "5000",
			CheermoteTierLevel.Cheer10K => "10000",
			CheermoteTierLevel.Cheer100K => "100000",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}

