using Koishibot.Core.Features.ChatCommands.Extensions;

namespace Koishibot.Core.Features.Dandle.Models;

public class DandleWord : IEntity
{
	public int Id { get; set; }
	public string Word { get; set; } = null!;

	public List<DandleResult> DandleResults { get; set; } = [];

	public DandleWord Set(string word)
	{
		Word = word;
		return this;
	}

	public DandleWord Set(int id)
	{
		Id = id;
		return this;
	}
}