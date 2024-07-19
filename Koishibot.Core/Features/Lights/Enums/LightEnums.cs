namespace Koishibot.Core.Features.Lights.Enums;


/// 
/// LightMode
/// 60 - Custom
/// 41, 61, 62 with white = 0 is Color
/// 41, 61, 62 with white != 0 is WarmWhite
/// 2a, 2b, 2c, 2d, 2e, 2f is Preset
/// 
/// Speed: 1-49

public enum TransitionType { Gradual = 0x3A, Jump = 0x3B, Strobe = 0x3C }
public enum LedProtocol { LEDENET, LEDENET_ORIGINAL, Unknown }
public enum PatternMode
{
	Color, WarmWhite, Preset, Custom, Unknown, SingleColor = 0x61
} // Ledmode
public enum PresetPattern
{
	RainbowCrossFade = 0x25,
	RedGradualChange = 0x26,
	GreenGradualChange = 0x27,
	BlueGradualChange = 0x28,
	YellowGradualChange = 0x29,
	CyanGradualChange = 0x2a,
	PurpleGradualChange = 0x2b,
	WhiteGradualChange = 0x2c,
	RedGreenCrossFade = 0x2d,
	RedBlueCrossFade = 0x2e,
	GreenBlueCrossFade = 0x2f,
	RainbowStrobe = 0x30,
	RedStrobeFlash = 0x31,
	GreenStrobeFlash = 0x32,
	BlueStrobeFlash = 0x33,
	YellowStrobeFlash = 0x34,
	CyanStrobeFlash = 0x35,
	PurpleStrobeFlash = 0x36,
	WhiteStrobeFlash = 0x37,
	RainbowFlash = 0x38,
	Normal = 0x61,
	Custom = 0x60,
	Setup = 0x63
}

public enum BulbType
{
	Bulb = 0x44,
	Tape = 0x33,
	BulbWhite = 0x35
}

//public static class BulbTypesExtension
//{
//	public static string Convert(this BulbType bulbType)
//	{
//		switch (bulbType)
//		{
//			case BulbType.RGBWW:
//				return "rgbww";
//			case BulbType.TAPE:
//				return "tape";
//			case BulbType.RGBWWCW:
//				return "rgbwwcw";
//			default:
//				return "UNKNOWN";
//		}
//	}
//}

public enum QueryStatusEnum
{
	QUERY_STATUS_1 = 0x81,
	QUERY_STATUS_2 = 0x8A,
	QUERY_STATUS_3 = 0x8B
}

public enum ResponseLen
{
	QueryStatus = 14,
	RESPONSE_LEN_SET_COLOR = 1,
	RESPONSE_LEN_CHANGE_MODE = 1,
	RESPONSE_LEN_CUSTOM_MODE = 0,
	QueryCustomMode = 70,
	QueryCurrentTime = 12,
	QueryTimers = 94,
	Power = 4
}

public enum CustomModeTerminatorEnum
{
	CUSTOM_MODE_TERMINATOR_1 = 0xFF,
	CUSTOM_MODE_TERMINATOR_2 = 0xF0
}

public enum PowerEnum
{
	TURN_ON_1 = 0x71,
	TURN_ON_2 = 0x23,
	TURN_ON_3 = 0x0F,
	TURN_OFF_1 = 0x71,
	TURN_OFF_2 = 0x24,
	TURN_OFF_3 = 0x0F
}

public enum BoolEnum
{
	TRUE = 0x0F,
	FALSE = 0xF0
}

public enum PowerStatusEnum
{
	ON = 0x23,
	OFF = 0x24
}


//QUERY_STATUS_1 = 0x81
//QUERY_STATUS_2 = 0x8A
//QUERY_STATUS_3 = 0x8B
//RESPONSE_LEN_QUERY_STATUS = 14

//SET_COLOR = 0x31
//RESPONSE_LEN_SET_COLOR = 1

//CHANGE_MODE = 0x61
//RESPONSE_LEN_CHANGE_MODE = 1

//CUSTOM_MODE = 0x51
//RESPONSE_LEN_CUSTOM_MODE = 0

//CUSTOM_MODE_TERMINATOR_1 = 0xFF
//CUSTOM_MODE_TERMINATOR_2 = 0xF0

//TURN_ON_1 = 0x71
//TURN_ON_2 = 0x23
//TURN_ON_3 = 0x0F
//TURN_OFF_1 = 0x71
//TURN_OFF_2 = 0x24
//TURN_OFF_3 = 0x0F
//RESPONSE_LEN_POWER = 4


//TRUE = 0x0F
//FALSE = 0xF0
//ON = 0x23
//OFF = 0x24