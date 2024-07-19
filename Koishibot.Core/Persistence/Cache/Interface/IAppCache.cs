﻿using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Persistence.Cache.Interface;
public interface IAppCache
{
	void Add<T>(CacheName name, T item);
	void Add<T>(CacheName name, T item, TimeSpan expireAt);
	void AddNoExpire<T>(CacheName name, T item);
	T? Get<T>(CacheName name) where T : class;
	void InitializeServices();
	Task UpdateServiceStatus(ServiceName name, bool status);
	void Remove(CacheName name);
}
