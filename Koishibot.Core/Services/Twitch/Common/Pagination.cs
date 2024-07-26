using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Common;

public class Pagination
{
    ///<summary>
    ///The cursor used to get the next page of results. <br/>
    ///Use the cursor to set the request’s after query parameter.
    ///</summary>
    [JsonPropertyName("cursor")]
    public string Cursor { get; set; }
}
