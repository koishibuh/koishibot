namespace Koishibot.Core.Features.Application.Interfaces;

public interface IStreamOnlineApi
{
    Task<bool> IsStreamOnline();
}