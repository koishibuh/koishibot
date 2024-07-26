namespace Koishibot.Core.Services.Twitch.EventSubs.Exceptions;

public class InvalidSubscriptionTypeException : Exception
{
    public InvalidSubscriptionTypeException(string? message) : base(message)
    {
    }
}