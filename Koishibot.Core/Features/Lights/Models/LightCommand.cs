using Koishibot.Core.Features.Lights.Enums;
namespace Koishibot.Core.Features.Lights.Models;

public class LightCommand
{
	public bool NeedsTerminator { get; set; } = true;
	public List<byte> Array { get; set; } = [];
	public int ResponseLength { get; set; } = 0;

	public string EnableLights()
	{
		Array = [(byte)0x71, (byte)0x23];
		ResponseLength = 4;
		return ConvertToHexString(Array);
	}

	public string DisableLights()
	{
		Array = [(byte)0x71, (byte)0x24];
		ResponseLength = 4;
		return ConvertToHexString(Array);
	}

	public string UpdateLightColor(Light light)
	{
		var whiteByte = light.IsWhite ? (byte)0x0F : (byte)0xF0;
		var terminator = (byte)0x0f;
		if (light.BulbType == BulbType.BulbWhite) // 0x0f is a terminator 
		{ // 0x31 CommandToSetColor
			Array.AddRange([0x31,
				(byte)light.Red, (byte)light.Green, (byte)light.Blue,
				(byte)light.White, (byte)light.CoolWhite, whiteByte, terminator]);
		}
		else
		{
			Array.AddRange([0x31,
				(byte)light.Red, (byte)light.Green, (byte)light.Blue,
				(byte)light.White, whiteByte, terminator]);
		}

		return ConvertToHexString(Array);
	}

	//public string CreateCustomMode()
	//{
	//	// 0x51 + color list + speed + (jump/gradual/strobe) +  0xFF + 0xF0


	//	//"""speed: float value 0 to 1.0

	//	//slowness: integer value 1 to 31"""
	//	//  slowness = int(-30 * value + 31)

	//	// color list needs to be 16 count; if too many remove, if too few, add more
	//}

	//public List<(int, int, int)> TrimColorsList(List<(int, int, int)> colors)
	//{
	//	var blankColors = new List<(int, int, int)>();
	//	if (colors.Count < 16)
	//	{
	//		var diff = 16 - colors.Count;
	//		return colors.Concat(Enumerable.Repeat(blankColors, diff)).ToList();
	//	}

	//	return colors.Take(16).ToList();


	//}

	public string SetCustomPattern(List<Color> colors, TransitionType transition, byte speed)
	{
		var data = new List<byte> { 0x51 };

		for (int i = 0; i < colors.Count; i++)
		{
			data.Add(0); // Add the separator byte
			data.AddRange(new byte[] { colors[i].Red, colors[i].Green, colors[i].Blue });
		}

		data.AddRange(Enumerable.Repeat(new byte[] { 0, 1, 2, 3 }, 16 - colors.Count).SelectMany(x => x));
		data.AddRange([0x00, SpeedToDelay(speed), Convert.ToByte(transition), 0xff, 0x0f]);

		var dataReady = data.ToArray();
		return "todo";
	}

	public byte SpeedToDelay(byte speed)
	{
		if (speed > 100)
			throw new Exception("Speed cannot have a value more than 100.");

		int inv_speed = 100 - speed;
		byte delay = Convert.ToByte((inv_speed * (0x1f - 1)) / 100);
		delay += 1;

		return delay;
	}


	public string QueryStatus() // LedProtocol.LEDENET
	{
		Array = [(byte)0x81, (byte)0x8A, (byte)0x8B];
		ResponseLength = 14; //ResponseLen.QueryStatus;
		NeedsTerminator = false;
		return "todo";
	}

	public string QueryTimers()
	{
		Array = [(byte)0x22, (byte)0x2A, (byte)0x2B];
		ResponseLength = 94; //ResponseLen.QueryTimers;
		return "todo";
	}

	public string QueryCurrentTimer()
	{
		Array = [(byte)0x11, (byte)0x1A, (byte)0x1B];
		ResponseLength = 12; //ResponseLen.QueryCurrentTime;
		return "todo";
	}

	public string QueryCustomMode()
	{
		Array = [(byte)0x52, (byte)0x5A, (byte)0x5B];
		ResponseLength = 70; //ResponseLen.QueryCustomMode;
		return "todo";
	}

	public string ConvertToHexString(List<byte> array)
	{
		var appended = AppendTerminator(array, true);
		var checksumAdded = AppendChecksum(appended);

		return string.Join("", checksumAdded.Select(v => v.ToString("X2")));
	}

	public List<byte> AppendTerminator(List<byte> array, bool convert)
	{
		var IsRemote = true;
		var terminator = IsRemote ? (byte)0x0F : (byte)0xF0;
		if (convert)
		{
			array.Add(terminator);
		}

		return array;
	}

	public List<byte> AppendChecksum(List<byte> array)
	{
		var checksum = CalculateChecksum(array);
		array.Add(checksum);
		return array;
	}

	public byte CalculateChecksum(List<byte> array)
	{
		var sum = array.Sum(b => b);
		// bitwise & to get last two bytes of sum
		var lastByte = sum & 0xFF;
		return (byte)lastByte;
	}
}
