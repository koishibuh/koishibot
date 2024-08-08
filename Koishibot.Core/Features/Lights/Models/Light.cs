using Koishibot.Core.Features.Lights.Enums;

namespace Koishibot.Core.Features.Lights.Models;

public record Light()
{
	public BulbType BulbType { get; set; }
	public string Name { get; set; } = string.Empty;
	public string MacAddress { get; set; } = string.Empty;
	public bool Power { get; set; }
	public int Speed { get; set; } = 1;
	public bool AllowFading { get; set; } = true;

	public PatternMode Mode { get; set; }
	public int Red { get; set; }
	public int Green { get; set; }
	public int Blue { get; set; }
	public int White { get; set; } = 0;
	public int CoolWhite { get; set; }
	public bool IsWhite { get; set; } = false;
	//Brightness 0 - 100;
	//LightMode : Color, Preset, White, Custom
	// Todo: find what requests do not require a checksum

	// == ⚫ == //

	public void UpdateRed(int r) => Red = RoundValue(r);
	public void UpdateGreen(int g) => Green = RoundValue(g);
	public void UpdateBlue(int b) => Blue = RoundValue(b);
	public void UpdateWhite(int w) => White = RoundValue(w);
	public void UpdateCoolWhite(int cw) => CoolWhite = RoundValue(cw);

	public Light UpdateRGB(int red, int green, int blue)
	{
		Red = RoundValue(red);
		Green = RoundValue(green);
		Blue = RoundValue(blue);
		return this;
	}

	public string GetHexCode()
	{
		return $"{Red}{Green}{Blue}";
	}

	public Light SetInitialValues(DeviceInfo deviceInfo)
	{
		Name = deviceInfo.DeviceName;
		MacAddress = deviceInfo.MacAddress;

		if (deviceInfo.State is null) // Light is off
		{
			return this;
		}
		else
		{
			string hex = deviceInfo.State;

			var type = hex.Substring(2, 2);
			BulbType = type is "33" ? BulbType.Tape : BulbType.BulbWhite;

			var power = hex.Substring(4, 2);
			Power = power is "23" ? true : false;

			Speed = Convert.ToInt32(hex.Substring(10, 2), 16);
			// Mode? int.Parse(hex.Substring(6, 2));

			Red = Convert.ToInt32(hex.Substring(12, 2), 16);
			Green = Convert.ToInt32(hex.Substring(14, 2), 16);
			Blue = Convert.ToInt32(hex.Substring(16, 2), 16);
			White = Convert.ToInt32(hex.Substring(18, 2), 16);
			CoolWhite = Convert.ToInt32(hex.Substring(20, 2), 16);
			return this;
		}
	}

	public int RoundValue(int value, int min = 0, int max = 255)
	{
		if (double.IsNaN(value) || double.IsInfinity(value))
		{
			throw new ArgumentException("Invalid value", nameof(value));
		}
		return Math.Min(Math.Max(value, min), max);
	}

}