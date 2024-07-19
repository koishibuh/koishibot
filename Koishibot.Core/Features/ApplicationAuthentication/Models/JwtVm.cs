using System.Security.Claims;

namespace Koishibot.Core.Features.ApplicationAuthentication.Models;

public record JwtVm(
	string Token,
	List<Claim> Claims	
);