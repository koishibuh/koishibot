using Koishibot.Core.Features.ChatCommands.Extensions;
using System.Text.Json;

namespace Koishibot.Core.Features.ChatCommands.Models;
public class ChatCommand : IEntity
{
	public int Id { get; set; }
	public string Description { get; set; } = string.Empty;
	public bool Enabled { get; set; }
	public string Message { get; set; } = null!;
	public PermissionLevel Permissions { get; set; }
	public TimeSpan UserCooldown { get; set; }
	public TimeSpan GlobalCooldown { get; set; }

	public List<CommandName> CommandNames { get; set; } = [];
	public List<TimerGroup>? TimerGroups { get; set; } = [];
}

[JsonConverter(typeof(PermissionLevelEnumConverter))]
public enum PermissionLevel
{
	App = 1,
	Broadcaster = 2,
	Mod = 3,
	Vip = 4,
	Everyone = 5
}

public class TimerGroup : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public TimeSpan Interval { get; set; }

	public List<ChatCommand> Commands { get; } = [];
}

public class CommandTimerGroup
{
	public int ChatCommandId { get; set; }
	public int TimerGroupId { get; set; }
}

public class CommandName : IEntity
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public int? ChatCommandId { get; set; }
	public ChatCommand? ChatCommand { get; set; }
}

// == ⚫ == //

public class PermissionLevelEnumConverter : JsonConverter<PermissionLevel>
{
	public override PermissionLevel Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"App" => PermissionLevel.App,
			"Broadcaster" => PermissionLevel.Broadcaster,
			"Mod" => PermissionLevel.Mod,
			"Vip" => PermissionLevel.Vip,
			"Everyone" => PermissionLevel.Everyone,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, PermissionLevel value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PermissionLevel.App => "App",
			PermissionLevel.Broadcaster => "Broadcaster",
			PermissionLevel.Mod => "Mod",
			PermissionLevel.Vip => "Vip",
			PermissionLevel.Everyone => "Everyone",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}