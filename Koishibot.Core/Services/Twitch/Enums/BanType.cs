using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(BanTypeEnumConverter))]
public enum BanType
{
	ManuallyAdded = 1,
	BanEvader,
	BannedInSharedChannel
}

// == ⚫ == //

public class BanTypeEnumConverter : JsonConverter<BanType>
{
	public override BanType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"manually_added" => BanType.ManuallyAdded,
			"ban_evader" => BanType.BanEvader,
			"banned_in_shared_channel" => BanType.BannedInSharedChannel,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, BanType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BanType.ManuallyAdded => "manually_added",
			BanType.BanEvader => "ban_evader",
			BanType.BannedInSharedChannel => "banned_in_shared_channel",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}