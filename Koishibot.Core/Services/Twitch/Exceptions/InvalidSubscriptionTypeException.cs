namespace Koishibot.Core.Services.Twitch.Exceptions;

public class InvalidSubscriptionTypeException : Exception
{
    public InvalidSubscriptionTypeException(string? message) : base(message)
    {
    }
}