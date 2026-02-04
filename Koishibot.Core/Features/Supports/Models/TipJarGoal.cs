using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Supports.Controllers;
using Koishibot.Core.Persistence;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Koishibot.Core.Features.Supports.Models;


/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class TipJarGoal : IEntity
{
	public int Id { get; set; }
	public DateTimeOffset? StartedOn { get; set; }
	public DateTimeOffset? EndedOn { get; set; }
	public string Title { get; set; }
	public int CurrentAmount { get; set; }
	public int GoalAmount { get; set; }
	public bool IsActive { get; set; }
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/

public static class TipJarGoalExtensions
{
	public static async Task<bool> HasActiveGoal(this KoishibotDbContext database) =>
		await database.TipJarGoals.AnyAsync(x => x.IsActive == true);
	
	public static async Task<TipJarGoalVm?> GetActiveTipJarGoalVm(this KoishibotDbContext database) =>
		await database.TipJarGoals
			.AsNoTracking()
			.Where(x => x.IsActive && x.EndedOn == null)
			.OrderByDescending(x => x.Id)
			.Select(x => new TipJarGoalVm(x.Title, x.CurrentAmount, x.GoalAmount))
			.FirstOrDefaultAsync();
	
	public static async Task<TipJarGoal?> GetActiveTipJarGoal(this KoishibotDbContext database) =>
		await database.TipJarGoals
			.Where(x => x.IsActive && x.EndedOn == null)
			.OrderByDescending(x => x.Id)
			.FirstOrDefaultAsync();
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class TipJarGoalConfig : IEntityTypeConfiguration<TipJarGoal>
{
	public void Configure(EntityTypeBuilder<TipJarGoal> builder)
	{
		builder.ToTable("TipJarGoals");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id);

		builder.Property(p => p.StartedOn);
		
		builder.Property(p => p.EndedOn);
		
		builder.Property(p => p.Title);
		builder.Property(p => p.CurrentAmount);
		builder.Property(p => p.GoalAmount);
		builder.Property(p => p.IsActive);
	}
}