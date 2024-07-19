using Newtonsoft.Json;

namespace Koishibot.Core.Features.Dandle.Models;

public class DandleDefinition
{
	[JsonProperty("word")]
	public string Word { get; set; } = string.Empty;

	[JsonProperty("phonetic")]
	public string Phonetic { get; set; } = string.Empty;

	[JsonProperty("phonetics")]
	public List<Phonetics> Phonetics { get; set; } = new();

	[JsonProperty("meanings")]
	public List<Meanings> Meanings { get; set; } = new();
}

public class Phonetics
{
	[JsonProperty("text")]
	public string Text { get; set; } = string.Empty;

	[JsonProperty("audio")]
	public string? Audio { get; set; } = string.Empty;
}
public class Meanings 
{	
	[JsonProperty("partOfSpeech")]
	public string PartOfSpeech { get; set; } = string.Empty;

	[JsonProperty("definitions")]
	public List<WordDefinition> Definitions { get; set; } = new();
}


public class WordDefinition
{
	[JsonProperty("definition")]
	public string Definition { get; set; } = string.Empty;
}