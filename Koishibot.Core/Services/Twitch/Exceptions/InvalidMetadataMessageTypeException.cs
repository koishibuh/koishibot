namespace Koishibot.Core.Services.Twitch.Exceptions;

public class InvalidMetadataMessageTypeException : Exception
{
    public InvalidMetadataMessageTypeException(string? message) : base(message)
    {
    }
}