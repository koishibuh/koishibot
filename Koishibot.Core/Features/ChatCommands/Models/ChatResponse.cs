using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Features.ChatCommands.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class ChatResponse : IEntity
{
	public int Id { get; set; }
	public string? InternalName { get; set; }
	public string? Description { get; set; }
	public string Message { get; set; }
	public List<ChatCommand> Commands { get; set; } = [];
	
}

/*══════════════════【 DTO 】═════════════════*/

public record ChatResponseDto(int Id, string InternalName, string Description, string Message);

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class ChatResponseConfig : IEntityTypeConfiguration<ChatResponse>
{
	public void Configure(EntityTypeBuilder<ChatResponse> builder)
	{
		builder.ToTable("ChatResponses");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.InternalName)
			.IsRequired(false)
			.HasMaxLength(500);
			
		builder
			.HasIndex(p => p.InternalName)
			.IsUnique();

		builder.Property(p => p.Description);
		
		builder
			.Property(p => p.Message)
			.HasMaxLength(500)
			.IsRequired();
		
		builder
			.HasMany(p => p.Commands)
			.WithOne(p => p.Response)
			.HasForeignKey(p => p.ResponseId)
			.IsRequired(false);
	}
}