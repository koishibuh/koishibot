using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Twitch Documentation</see><br/>
/// When any user sends a message to a specific chat room.<br/>
/// Required Scopes: user:read:chat<br/>
/// </summary>
public class ChatMessageReceivedEvent
{
    ///<summary>
    ///The broadcaster user ID.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The broadcaster login.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user ID of the user that sent the message.
    ///</summary>
    [JsonPropertyName("chatter_user_id")]
    public string ChatterUserId { get; set; }

    ///<summary>
    ///The user name of the user that sent the message.
    ///</summary>
    [JsonPropertyName("chatter_user_name")]
    public string ChatterUserName { get; set; }

    ///<summary>
    ///The user login of the user that sent the message.
    ///</summary>
    [JsonPropertyName("chatter_user_login")]
    public string ChatterUserLogin { get; set; }

    ///<summary>
    ///A UUID that identifies the message.
    ///</summary>
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; }

    ///<summary>
    ///The structured chat message.
    ///</summary>
    [JsonPropertyName("message")]
    public Message Message { get; set; }

    ///<summary>
    ///The type of message.
    ///</summary>
    [JsonPropertyName("message_type")]
    public MessageType MessageType { get; set; }

    ///<summary>
    ///List of chat badges.
    ///</summary>
    [JsonPropertyName("badges")]
    public List<Badge> Badges { get; set; }

    ///<summary>
    ///Optional. Metadata if this message is a cheer.
    ///</summary>
    [JsonPropertyName("cheer")]
    public Cheer? Cheer { get; set; }

    ///<summary>
    ///The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #&lt;RGB&gt;. This tag may be empty if it is never set.
    ///</summary>
    [JsonPropertyName("color")]
    public string Color { get; set; }

    ///<summary>
    ///Optional. Metadata if this message is a reply.
    ///</summary>
    [JsonPropertyName("reply")]
    public Reply? Reply { get; set; }

    ///<summary>
    ///Optional. The ID of a channel points custom reward that was redeemed.
    ///</summary>
    [JsonPropertyName("channel_points_custom_reward_id")]
    public string? ChannelPointsCustomRewardId { get; set; }

    ///<summary>
    ///Optional. An ID for the type of animation selected as part of an “animate my message” redemption.
    ///</summary>
    [JsonPropertyName("channel_points_animation_id")]
    public string? ChannelPointsAnimationId { get; set; }
}