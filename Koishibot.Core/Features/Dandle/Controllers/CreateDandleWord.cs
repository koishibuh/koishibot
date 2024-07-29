using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Dandle.Controllers;

// == ⚫ POST  == //

public class CreateDandleWordController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dandle"])]
	[HttpPost("/api/dandle/word")]
	public async Task<ActionResult> CreateDandleWord
			([FromBody] CreateDandleWordCommand e)
	{
		var result = await Mediator.Send(e);
		return Ok(result);
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// 
/// </summary>
public record CreateDandleWordHandler(
	KoishibotDbContext Database
	) : IRequestHandler<CreateDandleWordCommand, int>
{
	public async Task<int> Handle
		(CreateDandleWordCommand command, CancellationToken cancel)
	{
		var word = command.CreateModel();
		return await Database.UpdateEntry(word);
	}
}

// == ⚫ COMMAND  == //

public record CreateDandleWordCommand(
	string DandleWord
	) : IRequest<int>
{
	public DandleWord CreateModel() => new DandleWord{ Word = DandleWord };

	public async Task<bool> IsDandleWordUnique(KoishibotDbContext database)
	{
		var result = await database.DandleWords
			.FirstOrDefaultAsync(p => p.Word == DandleWord);

		return result is null;
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
