//using Koishibot.Core.Features.Polls.Events;
//using Koishibot.Core.Persistence.Cache.Enums;

//namespace Koishibot.Core.Features.Polls.Models;

//public class CurrentPoll
//{
//	public string Id { get; set; } = null!;
//	public string Title { get; set; } = string.Empty;
//	public DateTimeOffset StartedAt { get; set; }
//	public DateTimeOffset EndingAt { get; set; }
//	public TimeSpan Duration { get; set; }
//	public Dictionary<string, int> Choices { get; set; } = [];

//	public TimeSpan CalculateDuration()
//	{
//		return StartedAt - EndingAt;
//	}

//	public CurrentPoll ConvertFromEvent(PollStartedEvent e)
//	{
//		Id = e.Id;
//		Title = e.Title;
//		StartedAt = e.StartedAt;
//		EndingAt = e.EndingAt;
//		Choices = e.Choices;
//		return this;
//	}

//	public CurrentPoll ConvertFromEvent(VoteReceivedEvent e)
//	{
//		Id = e.Id;
//		Title = e.Title;
//		StartedAt = e.StartedAt;
//		EndingAt = e.EndingAt;
//		Choices = e.Choices;
//		return this;
//	}

//	public CurrentPoll ConvertFromEvent(PollEndedEvent e)
//	{
//		Id = e.Id;
//		Title = e.Title;
//		StartedAt = e.StartedAt;
//		EndingAt = e.EndingAt;
//		Choices = e.Choices;
//		return this;
//	}

//	public PollVm ConvertToVm()
//	{
//		return new PollVm(Id, Title, StartedAt, EndingAt, Duration, Choices);
//	}
//};