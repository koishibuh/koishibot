using Koishibot.Core.Features.Common.Enums;
namespace Koishibot.Core.Features.Common.Models;


public record OverlayStatus(
	OverlayName EnumName,
	bool Status
	)
{
	public string Name => EnumName.ToString();
}