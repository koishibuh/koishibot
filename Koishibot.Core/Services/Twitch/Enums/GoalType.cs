using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

// [JsonConverter(typeof(GoalTypeEnumConverter))]
public static class GoalType
{
	public const string Follow = "follow";
	public const string Follower = "follower";
	public const string Subscription = "subscription";
	public const string SubscriptionCount = "subscription_count";
	public const string NewSubscription = "new_subscription";
	public const string NewSubscriptionCount = "new_subscription_count";
	public const string NewBit = "new_bit";
	public const string NewCheerer = "new_cheerer";
}

// == ⚫ == //

public class GoalTypeEnumConverter : JsonConverter<string>
{
	public override string Read
	(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
		"follow" => GoalType.Follow,
		"follower" => GoalType.Follower,
		"subscription" => GoalType.Subscription,
		"subscription_count" => GoalType.SubscriptionCount,
		"new_subscription" => GoalType.NewSubscription,
		"new_subscription_count" => GoalType.NewSubscriptionCount,
		"new_bit" => GoalType.NewBit,
		"new_cheerer" => GoalType.NewCheerer,
		_ => throw new JsonException()
		};
	}

	public override void Write
	(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
		GoalType.Follow => "follow",
		GoalType.Follower => "follower",
		GoalType.Subscription => "subscription",
		GoalType.SubscriptionCount => "subscription_count",
		GoalType.NewSubscription => "new_subscription",
		GoalType.NewSubscriptionCount => "new_subscription_count",
		GoalType.NewBit => "new_bit",
		GoalType.NewCheerer => "new_cheerer",
		_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}