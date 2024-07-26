using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

//time— Sort the results in descending order by when they were created(i.e., latest video first).
//trending — Sort the results in descending order by biggest gains in viewership(i.e., highest trending video first).
//views — Sort the results in descending order by most views(i.e., highest number of views first).

public enum VideoSort
{
	Time = 1,
	Trending,
	Views
}


// == ⚫ == //

public class VideoSortEnumConverter : JsonConverter<VideoSort>
{
	public override VideoSort Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"time" => VideoSort.Time,
			"trending" => VideoSort.Trending,
			"views" => VideoSort.Views,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, VideoSort value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			VideoSort.Time => "time",
			VideoSort.Trending => "trending",
			VideoSort.Views => "views",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}