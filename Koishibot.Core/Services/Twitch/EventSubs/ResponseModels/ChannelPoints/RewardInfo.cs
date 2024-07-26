﻿using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
public class RewardInfo
{
    ///<summary>
    ///The reward identifier.
    ///</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    ///<summary>
    ///The reward name.
    ///</summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    ///<summary>
    ///The reward cost.
    ///</summary>
    [JsonPropertyName("cost")]
    public int Cost { get; set; }

    ///<summary>
    ///The reward description.
    ///</summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

}
