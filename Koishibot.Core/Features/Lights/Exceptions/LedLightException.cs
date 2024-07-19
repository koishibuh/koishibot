namespace Koishibot.Core.Features.Lights.Exceptions;

public class LedLightException : Exception
{
	new public string Message { get; private set; }
	public LedLightException(string message) => Message = message;
}
