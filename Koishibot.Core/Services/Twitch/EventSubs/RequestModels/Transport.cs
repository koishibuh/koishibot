﻿using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Transport
{
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
}