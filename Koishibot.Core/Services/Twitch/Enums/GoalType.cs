using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(GoalTypeEnumConverter))]
public enum GoalType
{
	Follow = 1,
	Follower,
	Subscription,
	SubscriptionCount,
	NewSubscription,
	NewSubscriptionCount,
	NewBit,
	NewCheerer
}

// == ⚫ == //

public class GoalTypeEnumConverter : JsonConverter<GoalType>
{
	public override GoalType Read
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
			(Utf8JsonWriter writer, GoalType value, JsonSerializerOptions options)
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

