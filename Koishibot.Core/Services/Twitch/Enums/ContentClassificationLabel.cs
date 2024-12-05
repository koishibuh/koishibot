using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


[JsonConverter(typeof(ContentClassificationLabelEnumConverter))]
public enum ContentClassificationLabel
{
	DrugsIntoxication = 1,
	Gambling,
	MatureGame,
	ProfanityVulgarity,
	SexualThemes,
	ViolentGraphic,
	DebatedSocialIssuesAndPolitics
}

// == ⚫ == //

public class ContentClassificationLabelEnumConverter : JsonConverter<ContentClassificationLabel>
{
	public override ContentClassificationLabel Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"DrugsIntoxication" => ContentClassificationLabel.DrugsIntoxication,
			"Gambling" => ContentClassificationLabel.Gambling,
			"MatureGame" => ContentClassificationLabel.MatureGame,
			"ProfanityVulgarity" => ContentClassificationLabel.ProfanityVulgarity,
			"SexualThemes" => ContentClassificationLabel.SexualThemes,
			"ViolentGraphic" => ContentClassificationLabel.ViolentGraphic,
			"DebatedSocialIssuesAndPolitics" => ContentClassificationLabel.DebatedSocialIssuesAndPolitics,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, ContentClassificationLabel value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ContentClassificationLabel.DrugsIntoxication => "DrugsIntoxication",
			ContentClassificationLabel.Gambling => "Gambling",
			ContentClassificationLabel.MatureGame => "MatureGame",
			ContentClassificationLabel.ProfanityVulgarity => "ProfanityVulgarity",
			ContentClassificationLabel.SexualThemes => "SexualThemes",
			ContentClassificationLabel.ViolentGraphic => "ViolentGraphic",
			ContentClassificationLabel.DebatedSocialIssuesAndPolitics => "DebatedSocialIssuesAndPolitics",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}