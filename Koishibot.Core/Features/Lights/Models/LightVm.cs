namespace Koishibot.Core.Features.Lights.Models;
public record LightVm(
	string LightName,
	bool Power,
	string Color,
	bool IsSelected);
