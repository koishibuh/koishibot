using System.Text.Json;
namespace Koishibot.Core.Services.OBS.Sources;

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getinputlist">Obs Documentation</see>
/// </summary>
public class GetInputListRequest
{
	/// <summary>
	/// Optional. Restrict the array to only inputs of the specified kind
	/// </summary>
	public string? InputKind { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetInputListResponse
{
	/// <summary>
	/// Array of inputs
	/// </summary>
	public List<Input> Inputs { get; set; }
}

public class Input
{
	[JsonConverter(typeof(ObsInputKindConverter))]
	public string InputKind { get; set; }

	public string InputName { get; set; }

	public string InputUuid { get; set; }

	public string UnversionedInputKind { get; set; }
}

public class InputTypes
{
	public const string TextGdiPlus2 = "text_gdiplus_v2"; //Text
	public const string AudioOutputCapture = "wasapi_output_capture"; //Audio
	public const string AudioInputCapture = "wasapi_input_capture";
	public const string DeviceInput = "dshow_input"; // Console
	public const string ffmpegSource = "ffmpeg_source";
	public const string ImageSource = "image_source";
	public const string BrowserSource = "browser_source";
	public const string ColorSource = "color_source";
	public const string SourceClone = "source-clone";
	public const string WindowCapture = "window_capture";
	public const string MonitorCapture = "monitor_capture";
	public const string GameCapture = "game_capture";
}

public class ObsInputKindConverter : JsonConverter<string>
{

	public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var inputKind = reader.GetString();
		return inputKind switch
		{
			"text_gdiplus" => "Text", //Text
			"text_gdiplus_v2" => "Text", //Text
			"wasapi_output_capture" => "Audio Output", //Audio
			"wasapi_input_capture" => "Audio Input",
			"dshow_input" => "Device Input", // Console
			"ffmpeg_source" => "FFmpeg", // WIFI cam
			"image_source" => "Image",
			"browser_source" => "Browser",
			"color_source" => "Color",
			"color_source_v3" => "Color",
			"source-clone" => "Source",
			"window_capture" => "Window Capture",
			"monitor_capture" => "Monitor Capture",
			"game_capture" => "Game Capture",
			_ => "Unknown"
		};
	}


	public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}