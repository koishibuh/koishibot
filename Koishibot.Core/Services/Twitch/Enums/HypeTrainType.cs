using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

public static class HypeTrainType
{
	public const string Treasure = "treasure";
	public const string GoldenKappa = "golden_kappa";
	public const string Regular = "regular";
}

// == ⚫ == //

public class HypeTrainTypeConverter : JsonConverter<string>
{
	public override string Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"treasure" => HypeTrainType.Treasure,
			"golden_kappa" => HypeTrainType.GoldenKappa,
			"regular" => HypeTrainType.Regular,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			HypeTrainType.Treasure => "treasure",
			HypeTrainType.GoldenKappa => "golden_kappa",
			HypeTrainType.Regular => "regular",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}