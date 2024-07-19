namespace Koishibot.Core.Features.Lights.Models;
public class Color
{
	public byte Red { get; set; }
	public byte Green { get; set; }
	public byte Blue { get; set; }

	public Color Initialize(byte red, byte green, byte blue)
	{
		Red = red;
		Green = green;
		Blue = blue;
		return this;
	}

	public Color Initialize(string hexColor)
	{
		var byteArray = ConvertToByteArray(hexColor);
		Red = byteArray[0];
		Green = byteArray[1];
		Blue = byteArray[2];
		return this;
	}

	public byte[] ConvertToByteArray(string hexColor)
	{
		hexColor = hexColor.TrimStart('#');
		if (hexColor.Length % 2 != 0)
			throw new ArgumentException("Invalid hex string. It must have an even number of characters.");

		return Enumerable.Range(0, hexColor.Length / 2)
			.Select(x => Convert.ToByte(hexColor.Substring(x * 2, 2), 16))
			.ToArray();

	}
}
