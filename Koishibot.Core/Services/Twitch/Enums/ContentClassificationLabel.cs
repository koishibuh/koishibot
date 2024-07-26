﻿using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


public enum ContentClassificationLabel
{
	DrugsIntoxication = 1,
	Gambling,
	MatureGame,
	ProfanityVulgarity,
	SexualThemes,
	ViolentGraphic
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
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}

public class ContentClassificationLabelEnumListConverter : JsonConverter<List<ContentClassificationLabel>>
{
	public override List<ContentClassificationLabel> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var list = new List<ContentClassificationLabel>();
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndArray)
				return list;

			var value = reader.GetString();
			list.Add(value switch
			{
				"DrugsIntoxication" => ContentClassificationLabel.DrugsIntoxication,
				"Gambling" => ContentClassificationLabel.Gambling,
				"MatureGame" => ContentClassificationLabel.MatureGame,
				"ProfanityVulgarity" => ContentClassificationLabel.ProfanityVulgarity,
				"SexualThemes" => ContentClassificationLabel.SexualThemes,
				"ViolentGraphic" => ContentClassificationLabel.ViolentGraphic,
				_ => throw new JsonException()
			});
		}
		throw new JsonException();
	}

	public override void Write(Utf8JsonWriter writer, List<ContentClassificationLabel> value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var label in value)
		{
			var mappedValue = label switch
			{
				ContentClassificationLabel.DrugsIntoxication => "DrugsIntoxication",
				ContentClassificationLabel.Gambling => "Gambling",
				ContentClassificationLabel.MatureGame => "MatureGame",
				ContentClassificationLabel.ProfanityVulgarity => "ProfanityVulgarity",
				ContentClassificationLabel.SexualThemes => "SexualThemes",
				ContentClassificationLabel.ViolentGraphic => "ViolentGraphic",
				_ => throw new ArgumentOutOfRangeException(nameof(label), label, null)
			};
			writer.WriteStringValue(mappedValue);
		}
		writer.WriteEndArray();
	}
}