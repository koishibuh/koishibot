using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Dandle.Controllers;

// == ⚫ DELETE  == //

public class DeleteDandleWordController : ApiControllerBase
{
	[SwaggerOperation(Tags = new[] { "Dandle" })]
	[HttpDelete("/api/dandle/word")]
	public async Task<ActionResult> DeleteDandleWord
			([FromBody] DeleteDandleWordCommand e)
	{
		await Mediator.Send(e);
		return Ok();
	}
}

// == ⚫ COMMAND  == //

public record DeleteDandleWordCommand(
	int DandleWordId
	) : IRequest
{
	//public async Task<bool> IsDandleWordUnique(KoishibotDbContext database)
	//{
	//	var result = await database.DandleWords
	//		.FirstOrDefaultAsync(p => p.Word == DandleWord);

	//	return result is null;
	//}
}

// == ⚫ HANDLER  == //

/// <summary>
/// 
/// </summary>
public record DeleteDandleWordHandler(
	KoishibotDbContext Database
	) : IRequestHandler<DeleteDandleWordCommand>
{
	public async Task Handle
		(DeleteDandleWordCommand c, CancellationToken cancel)
	{
		var word = new DandleWord().Set(c.DandleWordId);
		await Database.RemoveDandleWord(word);
	}
}

// == ⚫ VALIDATOR == //

//public class DeleteDandleWordValidator
//	: AbstractValidator<CreateDandleWordCommand>
//{
//	public KoishibotDbContext Database { get; }

//	public DeleteDandleWordValidator(KoishibotDbContext context)
//	{
//		Database = context;

//		RuleFor(p => p.DandleWord)
//			.MaximumLength(5)
//			.NotEmpty();

//		RuleFor(p => p)
//			.MustAsync(IsDandleWordUnique)
//			.WithMessage("Dandle word already exists");
//	}

//	private async Task<bool> IsDandleWordUnique
//			(CreateDandleWordCommand command, CancellationToken cancel)
//	{
//		return await command.IsDandleWordUnique(Database);
//	}
//}
