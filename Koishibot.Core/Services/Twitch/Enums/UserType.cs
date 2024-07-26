using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

public enum UserType
{
	Admin,
	GlobalMod,
	Staff,
	Default
}

// == ⚫ == //

public class UserTypeEnumConverter : JsonConverter<UserType>
{
	public override UserType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"admin" => UserType.Admin,
			"global_mod" => UserType.GlobalMod,
			"staff" => UserType.Staff,
			"" => UserType.Default,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, UserType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			UserType.Admin => "admin",
			UserType.GlobalMod => "global_mod",
			UserType.Staff => "staff",
			UserType.Default => "",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}