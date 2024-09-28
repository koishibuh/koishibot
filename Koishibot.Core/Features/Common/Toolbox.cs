using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Features.Common;

public static class Toolbox
{
	public static DateTimeOffset ConverUnixToDateTimeOffset(string UnixTime)
	{
		return DateTimeOffset.FromUnixTimeSeconds(long.Parse(UnixTime));
	}

	public static bool NextAdTimeStillValid(string unix)
	{
		var currentTime = DateTimeOffset.UtcNow;
		var nextAd = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unix));

		return currentTime < nextAd;
	}

	public static int GenerateRandomNumber(int max)
	{
		return new Random().Next(1, (max + 1));
	}

	public static bool NumberWithinRange(int number, int upperLimit)
	{
		return number <= upperLimit;
	}

	public static DateTime[] DateRangeOfLast30Days()
	{
		return [DateTime.UtcNow, DateTime.UtcNow.AddDays(-30)];
	}

	public static bool StringContainsNonLetters(string word)
	{
		var validCharacters = new Regex(@"^[a-zA-Z]+$");
		return !validCharacters.IsMatch(word);
	}

	public static ATimer CreateTimer(TimeSpan delay, Action onComplete)
	{
		var timer = new ATimer
		{
			AutoReset = false,
			Interval = delay.TotalMilliseconds
		};

		timer.Elapsed += (_, _) => { timer.Dispose(); onComplete(); };
		return timer;
	}

	public static ATimer CreateTimer(int delaySeconds, Action onComplete)
	{
		var seconds = TimeSpan.FromSeconds(delaySeconds);

		var timer = new ATimer
		{
		AutoReset = false,
		Interval = seconds.TotalMilliseconds
		};

		timer.Elapsed += (_, _) => { timer.Dispose(); onComplete(); };
		return timer;
	}

	public static string CreateRandom32CharString()
	{
		return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 32)
					.Select(s => s[new Random().Next(s.Length)]).ToArray());
	}

	public static string CreateMD5Hash(string password)
	{
		using (var md5 = MD5.Create())
		{
			var inputBytes = Encoding.UTF8.GetBytes(password);
			var hashBytes = md5.ComputeHash(inputBytes);

			var sb = new StringBuilder();
			for (var i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}
			return sb.ToString();
		}
	}

	public static StringContent CreateStringContent(object payload)
	{
		var jsonPayload = JsonConvert.SerializeObject(payload);
		return new StringContent(jsonPayload,	Encoding.UTF8, "application/json");
	}

	public static bool NotInDatabase<T>(this T? item) where T : class =>
		item is null;

	public static bool InCache<T>(this T? item) where T : class =>
		item is not null;

	public static bool IsEmpty<T>(this List<T>? list) =>
		list == null || list.Count == 0;

	public static string CreateUITimestamp() =>
		(DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm");
}