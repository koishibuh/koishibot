using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

//all
//archive — On-demand videos(VODs) of past streams.
//highlight — Highlight reels of past streams.
//upload — External videos that the broadcaster uploaded using the Video Producer.

[JsonConverter(typeof(VideoTypeEnumConverter))]
public enum VideoType
{
	All = 1,
	Archive,
	Highlight,
	Upload
}

// == ⚫ == //

public class VideoTypeEnumConverter : JsonConverter<VideoType>
{
	public override VideoType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"all" => VideoType.All,
			"archive" => VideoType.Archive,
			"highlight" => VideoType.Highlight,
			"upload" => VideoType.Upload,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, VideoType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			VideoType.All => "all",
			VideoType.Archive => "archive",
			VideoType.Highlight => "highlight",
			VideoType.Upload => "upload",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}