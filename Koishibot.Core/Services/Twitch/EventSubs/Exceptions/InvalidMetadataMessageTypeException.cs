namespace Koishibot.Core.Services.Twitch.EventSubs.Exceptions;

public class InvalidMetadataMessageTypeException : Exception
{
    public InvalidMetadataMessageTypeException(string? message) : base(message)
    {
    }
}