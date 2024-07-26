using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodsettingsupdate">Twitch Documentation</see><br/>
/// When a broadcaster’s automod settings are updated.<br/>
/// Required Scopes: moderator:read:automod_settings<br/>
/// </summary>
public class AutomodSettingsUpdate
{
    ///<summary>
    ///The ID of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The login of the broadcaster specified in the request. (Lowercase)
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The ID of the moderator who changed the channel settings.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    ///<summary>
    ///The moderator’s login. (Lowercase)
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

    ///<summary>
    ///The moderator’s user name.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }

    ///<summary>
    ///The Automod level for hostility involving name calling or insults.
    ///</summary>
    [JsonPropertyName("bullying")]
    public int Bullying { get; set; }

    ///<summary>
    ///The default AutoMod level for the broadcaster.<br/>
    ///This field is null if the broadcaster has set one or more of the individual settings.
    ///</summary>
    [JsonPropertyName("overall_level")]
    public int? OverallLevel { get; set; }

    ///<summary>
    ///The Automod level for discrimination against disability.
    ///</summary>
    [JsonPropertyName("disability")]
    public int Disability { get; set; }

    ///<summary>
    ///The Automod level for racial discrimination.
    ///</summary>
    [JsonPropertyName("race_ethnicity_or_religion")]
    public int RaceEthnicityOrReligion { get; set; }

    ///<summary>
    ///The Automod level for discrimination against women.
    ///</summary>
    [JsonPropertyName("misogyny")]
    public int Misogyny { get; set; }

    ///<summary>
    ///The AutoMod level for discrimination based on sexuality, sex, or gender.
    ///</summary>
    [JsonPropertyName("sexuality_sex_or_gender")]
    public int SexualitySexOrGender { get; set; }

    ///<summary>
    ///The Automod level for hostility involving aggression.
    ///</summary>
    [JsonPropertyName("aggression")]
    public int Aggression { get; set; }

    ///<summary>
    ///The Automod level for sexual content.
    ///</summary>
    [JsonPropertyName("sex_based_terms")]
    public int SexBasedTerms { get; set; }

    ///<summary>
    ///The Automod level for profanity.
    ///</summary>
    [JsonPropertyName("swearing")]
    public int Swearing { get; set; }
}