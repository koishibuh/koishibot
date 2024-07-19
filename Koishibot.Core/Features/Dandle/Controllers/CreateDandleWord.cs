using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Dandle.Controllers;

// == ⚫ POST  == //

public class CreateDandleWordController : ApiControllerBase
{
	[SwaggerOperation(Tags = new[] { "Dandle" })]
	[HttpPost("/api/dandle/word")]
	public async Task<ActionResult> CreateDandleWord
			([FromBody] CreateDandleWordCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

// == ⚫ COMMAND  == //

public record CreateDandleWordCommand(
	string DandleWord
	) : IRequest
{
	public async Task<bool> IsDandleWordUnique(KoishibotDbContext database)
	{
		var result = await database.DandleWords
			.FirstOrDefaultAsync(p => p.Word == DandleWord);

		return result is null;
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// 
/// </summary>
public record CreateDandleWordHandler(
	KoishibotDbContext Database
	) : IRequestHandler<CreateDandleWordCommand>
{
	public async Task Handle
		(CreateDandleWordCommand c, CancellationToken cancel)
	{
		var word = new DandleWord().Set(c.DandleWord);
		await Database.UpdateDandleWord(word);
	}
}

// == ⚫ VALIDATOR == //

public class AddDandleWordValidator
	: AbstractValidator<CreateDandleWordCommand>
{
	public KoishibotDbContext Database { get; }

	public AddDandleWordValidator(KoishibotDbContext context)
	{
		Database = context;

		RuleFor(p => p.DandleWord)
			.MaximumLength(5)
			.NotEmpty();

		RuleFor(p => p)
			.MustAsync(IsDandleWordUnique)
			.WithMessage("Dandle word already exists");
	}

	public async Task<bool> IsDandleWordUnique
			(CreateDandleWordCommand command, CancellationToken cancel)
	{
		return await command.IsDandleWordUnique(Database);
	}
}
