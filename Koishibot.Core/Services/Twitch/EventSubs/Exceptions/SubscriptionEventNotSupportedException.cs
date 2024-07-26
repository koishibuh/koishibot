namespace Koishibot.Core.Services.Twitch.EventSubs.Exceptions;

public class SubscriptionEventNotSupportedException : Exception
{
    public SubscriptionEventNotSupportedException(string? message) : base(message)
    {
    }
}