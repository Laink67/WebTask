using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace CacheLayer
{/// <summary>
///  Кэширование на уровне репозиториев
/// </summary>
/// <typeparam name="T"></typeparam>
    public class DefaultCacheProvider<T> where T : class
    {
        public ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        public bool IsInMemory(string Key)
        {
            return Cache.Contains(Key);
        }

        public void Add(string Key, object Value, int Expiration)
        {
            Cache.Set(Key, Value, new CacheItemPolicy().AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(Expiration));
        }
        public T Get(string Key)
        {
            return (T)Cache[Key];
        }

        public List<T> FetchData<T>(string Key) 
        {
            return Cache[Key] as List<T>;
        }

        public void Remove(string Key)
        {
            Cache.Remove(Key);
        }
    }
}
