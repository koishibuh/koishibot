//namespace Koishibot.Core.Features.Polls.Models;

//public class PendingPoll
//{
//	public string Title { get; set; } = string.Empty;
//	public List<string> Choices { get; set; } = [];
//	public int Duration { get; set; }

//	// == ⚫ == //

//	public PendingPoll Set(CreatePollCommand command)
//	{
//		Title = command.Title;
//		Choices = command.Choices;
//		Duration = command.Duration;
//		return this;
//	}

//	public PendingPoll CreateDadJokePoll(string username)
//	{
//		Title = $"How would you rate {username}'s Dad Joke?";
//		Choices = ["⭐⭐⭐⭐⭐", "⭐⭐⭐⭐", "⭐⭐⭐", "⭐⭐", "⭐"];
//		Duration = 120;

//		return this;
//	}
//};