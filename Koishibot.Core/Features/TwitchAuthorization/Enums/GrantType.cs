using System.Text.Json;
namespace Koishibot.Core.Features.TwitchAuthorization.Enums;

public enum GrantType
{
	AuthorizationCode,
	RefreshToken
}

// == ⚫ == //


public static class GrantTypeExtension
{
	public static string ConvertToString(this GrantType type)
	{
		return type switch
		{
			GrantType.AuthorizationCode => "authorization_code",
			GrantType.RefreshToken => "refresh_token",
			_ => "",
		};
	}
}



public class GrantTypeEnumConverter : JsonConverter<GrantType>
{
	public override GrantType Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"authorization_code" => GrantType.AuthorizationCode,
			"refresh_token" => GrantType.RefreshToken,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, GrantType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			GrantType.AuthorizationCode => "authorization_code",
			GrantType.RefreshToken => "refresh_token",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}