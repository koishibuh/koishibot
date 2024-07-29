namespace Koishibot.Core.Services.Twitch.Exceptions;

public class SubscriptionEventNotSupportedException : Exception
{
    public SubscriptionEventNotSupportedException(string? message) : base(message)
    {
    }
}