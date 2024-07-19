using Koishibot.Core.Features.Common.Models;

namespace Koishibot.Core.Features.TwitchUsers.Interfaces;
public interface IGetUserInfoByUsernameApi
{
	Task<UserInfo> GetUserInfoByUsername(string username);
}