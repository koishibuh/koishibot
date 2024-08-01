namespace Koishibot.Core.Features.Dandle.Models;
public record WordData(
	string Word);

public record WordDefineData(
	string Word,
	string Definition);


public record WordsTimeData(
	string Words,
	int Time
	);

public record TimeData(
	int Time);

public record NumberData(
	int Number);

public record GuessData(
	int Number,
	string Word,
	string Emoji);