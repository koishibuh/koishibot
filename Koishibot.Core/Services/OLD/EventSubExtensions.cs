//using Koishibot.Core.Features.ChannelPoints.Events;
//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Features.ChatMessages.Models;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Polls.Events;
//using Koishibot.Core.Features.TwitchUsers.Models;
//namespace Koishibot.Core.Services.TwitchEventSub.Extensions;

//public static class EventSubExtensions
//{
//	// == ⚫ Chat Irc

//	public static ChatMessageEvent ConvertToEvent(this OnMessageReceivedArgs e)
//	{
//		if (e.ChatMessage.ContainsCommandPrefix())
//		{
//			(var command, var message) = e.ChatMessage.Message.SplitCommandMessage();
//			return new ChatMessageEvent()
//								 .Set(e, command, message);
//		}
//		else
//		{
//			return new ChatMessageEvent()
//								 .Set(e, null, e.ChatMessage.Message);
//		}
//	}

//	public static ChatMessageEvent ConvertToEvent(this OnMessageSentArgs e)
//	{
//		if (e.SentMessage.ContainsCommandPrefix())
//		{
//			(var command, var message) = e.SentMessage.Message.SplitCommandMessage();
//			return new ChatMessageEvent()
//								 .Set(e, command, message);
//		}
//		else
//		{
//			return new ChatMessageEvent()
//								 .Set(e, null, e.SentMessage.Message);
//		}
//	}

//	public static bool ContainsCommandPrefix(this ChatMessage e)
//	{
//		return e.Message.StartsWith('!');
//	}

//	public static bool ContainsCommandPrefix(this SentMessage e)
//	{
//		return e.Message.StartsWith('!');
//	}

//	public static (string command, string message) SplitCommandMessage(this string chatMessage)
//	{
//		var trimmedMessage = chatMessage.Trim();
//		int firstSpaceIndex = trimmedMessage.IndexOf(' ');

//		if (firstSpaceIndex == -1) // !command (no message)
//		{
//			var command = trimmedMessage.Length > 0
//										? trimmedMessage.Substring(1).ToLower()
//										: throw new Exception("Error in SplitCommandMessage");
//			var message = "";

//			return (command, message);
//		}
//		else // !command message
//		{
//			var command = trimmedMessage
//										.Substring(1, firstSpaceIndex - 1)
//										.ToLower();

//			var message = trimmedMessage
//										.Substring(firstSpaceIndex + 1);

//			return (command, message);
//		}
//	}

//	// == ⚫ Channel EventSubs

//	public static StreamInfo ConvertToModel(this ChannelUpdateArgs args)
//	{
//		return new StreamInfo(
//			new TwitchUserDto(
//				args.Notification.Payload.Event.BroadcasterId,
//				args.Notification.Payload.Event.BroadcasterLogin,
//				args.Notification.Payload.Event.BroadcasterName),
//				args.Notification.Payload.Event.Title,
//				args.Notification.Payload.Event.CategoryName,
//				args.Notification.Payload.Event.CategoryId);
//	}

//	public static StreamInfo ConvertToDto(this GetChannelInformationResponse response)
//	{
//		var stream = response.Data[0];

//		return new StreamInfo(
//						new TwitchUserDto(
//										stream.BroadcasterId,
//										stream.BroadcasterLogin,
//										stream.BroadcasterName),
//						stream.GameName,
//						stream.GameId,
//						stream.Title
//						);
//	}

//	// == ⚫ Poll EventSubs

//	public static PollStartedEventModel ConvertToEvent(this ChannelPollBeginArgs args)
//	{
//		return new PollStartedEventModel(
//				args.Notification.Payload.Event.Id,
//				args.Notification.Payload.Event.Title,
//				args.Notification.Payload.Event.StartedAt,
//				args.Notification.Payload.Event.EndsAt,
//				AddVotesToDictionary(args.Notification.Payload.Event.Choices));
//	}

//	public static VoteReceivedEvent ConvertToEvent(this ChannelPollProgressArgs args)
//	{
//		return new VoteReceivedEvent(
//				args.Notification.Payload.Event.Id,
//				args.Notification.Payload.Event.Title,
//				args.Notification.Payload.Event.StartedAt,
//				args.Notification.Payload.Event.EndsAt,
//				AddVotesToDictionary(args.Notification.Payload.Event.Choices));
//	}

//	public static PollEndedEventModel ConvertToEvent(this ChannelPollEndArgs args)
//	{
//		return new PollEndedEventModel(
//				args.Notification.Payload.Event.Id,
//				args.Notification.Payload.Event.Title,
//				args.Notification.Payload.Event.StartedAt,
//				args.Notification.Payload.Event.EndedAt,
//				AddVotesToDictionary(args.Notification.Payload.Event.Choices));
//	}

//	public static Dictionary<string, int> AddVotesToDictionary
//			 (PollChoice[] pollChoices)
//	{
//		var pollResults = new Dictionary<string, int>();
//		foreach (var pollChoice in pollChoices)
//		{
//			pollResults[pollChoice.Title]
//					= pollResults.TryGetValue(pollChoice.Title, out int currentVoteCount)
//							? currentVoteCount + (pollChoice.Votes ?? 0)
//							: pollChoice.Votes ?? 0;
//			// ?? if null then 0; coalesce operator
//		}

//		return pollResults;
//	}

//	public static bool IsPollStatusArchive(this ChannelPollEndArgs args)
//	{
//		return args.Notification.Payload.Event.Status == "archived";
//	}

//	// == ⚫ ChannelPoint EventSubs

//	//public static ChannelPointReward ConvertToEntity(this CreateCustomRewardsResponse response)
//	//{
//	//	var reward = response.Data[0];

//	//	return new ChannelPointReward
//	//	{
//	//		TwitchId = reward.Id,
//	//		Title = reward.Title,
//	//		Description = reward.Prompt,
//	//		Cost = reward.Cost,
//	//		BackgroundColor = reward.BackgroundColor,
//	//		IsEnabled = reward.IsEnabled,
//	//		IsUserInputRequired = reward.IsUserInputRequired,
//	//		IsMaxPerStreamEnabled = reward.MaxPerStreamSetting.IsEnabled,
//	//		MaxPerStream = reward.MaxPerStreamSetting.MaxPerStream,
//	//		IsMaxPerUserPerStreamEnabled = reward.MaxPerUserPerStreamSetting.IsEnabled,
//	//		MaxPerUserPerStream = reward.MaxPerUserPerStreamSetting.MaxPerUserPerStream,
//	//		IsGlobalCooldownEnabled = reward.GlobalCooldownSetting.IsEnabled,
//	//		GlobalCooldownSeconds = reward.GlobalCooldownSetting.GlobalCooldownSeconds,
//	//		IsPaused = reward.IsPaused,
//	//		ShouldRedemptionsSkipRequestQueue = reward.ShouldRedemptionsSkipQueue,
//	//		ImageUrl = reward.Image is not null ? reward.Image.Url4x : reward.DefaultImage.Url4x
//	//	};
//	//}

//	public static List<ChannelPointReward> ConvertToEntity(this GetCustomRewardsResponse response)
//	{
//		return response.Data
//			.Select(reward => new ChannelPointReward
//			{
//				TwitchId = reward.Id,
//				Title = reward.Title,
//				Description = reward.Prompt,
//				Cost = reward.Cost,
//				BackgroundColor = reward.BackgroundColor,
//				IsEnabled = reward.IsEnabled,
//				IsUserInputRequired = reward.IsUserInputRequired,
//				IsMaxPerStreamEnabled = reward.MaxPerStreamSetting.IsEnabled,
//				MaxPerStream = reward.MaxPerStreamSetting.MaxPerStream,
//				IsMaxPerUserPerStreamEnabled = reward.MaxPerUserPerStreamSetting.IsEnabled,
//				MaxPerUserPerStream = reward.MaxPerUserPerStreamSetting.MaxPerUserPerStream,
//				IsGlobalCooldownEnabled = reward.GlobalCooldownSetting.IsEnabled,
//				GlobalCooldownSeconds = reward.GlobalCooldownSetting.GlobalCooldownSeconds,
//				IsPaused = reward.IsPaused,
//				ShouldRedemptionsSkipRequestQueue = reward.ShouldRedemptionsSkipQueue,
//				ImageUrl = reward.Image is not null ? reward.Image.Url4x : reward.DefaultImage.Url4x
//			})
//			.ToList();
//	}

//	public static RedeemedRewardEvent ConvertToChannelPointRedeemedEvent
//	(this ChannelPointsCustomRewardRedemptionArgs e)
//	{
//		var x = e.Notification.Payload.Event;

//		return new RedeemedRewardEvent(
//			new TwitchUserDto(x.UserId, x.UserLogin, x.UserName),
//			x.UserInput,
//			x.RedeemedAt,
//			x.Reward.Title,
//			x.Reward.Id,
//			x.Reward.Prompt,
//			x.Status,
//			x.Reward.Cost);
//	}

//	public static bool IsIncomingRaid(this ChannelRaidArgs raid)
//	{
//		return "98683749" == raid.Notification.Payload.Event.ToBroadcasterId;
//	}

//	public static (TwitchUserDto user, int viewerCount) ConvertToDto(this ChannelRaidArgs e)
//	{
//		var user = new TwitchUserDto(
//			e.Notification.Payload.Event.FromBroadcasterId,
//			e.Notification.Payload.Event.FromBroadcasterLogin,
//			e.Notification.Payload.Event.FromBroadcasterName);

//		var viewerCount = e.Notification.Payload.Event.Viewers;

//		return (user, viewerCount);
//	}


//	public static TwitchUserDto ConvertToDto(this ChannelFollowArgs args)
//	{
//		return new TwitchUserDto(
//			args.Notification.Payload.Event.UserId,
//			args.Notification.Payload.Event.UserLogin,
//			args.Notification.Payload.Event.UserName);
//	}

//	public static TwitchUserDto ConvertToDto(this ChannelCheerArgs args)
//	{
//		return new TwitchUserDto(
//			args.Notification.Payload.Event.UserId,
//			args.Notification.Payload.Event.UserLogin,
//			args.Notification.Payload.Event.UserName);
//	}
//}
