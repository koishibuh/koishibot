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
	public string InputKind { get; set; }
	public string InputName { get; set; }
	public string InputUuid { get; set; }
	public string UnversionedInputKind { get; set; }
}

public class InputTypes
{
	public const string TextGdiPlus2 = "text_gdiplus"; //Text
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