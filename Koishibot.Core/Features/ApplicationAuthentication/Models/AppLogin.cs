namespace Koishibot.Core.Features.ApplicationAuthentication.Models;

public record AppLogin(
	int Id,
	string Username,
	string HashedPassword
	);
