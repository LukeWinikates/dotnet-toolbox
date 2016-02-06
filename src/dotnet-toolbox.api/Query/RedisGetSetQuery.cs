using System;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public class RedisGetSetQuery<T> : IGetSetQuerier<T> where T : IRedisHashable, new() 
    {
        private IDatabase redisDatabase;
        private Func<string, RedisKey> keyBuilder;

        public RedisGetSetQuery(IDatabase redisDatabase, Func<string, RedisKey> keyBuilder)
        {
            this.redisDatabase = redisDatabase;
            this.keyBuilder = keyBuilder;
        }

        public T Get(string key)
        {
            var entries = redisDatabase.HashGetAll(keyBuilder(key));
            if (entries.Length == 0)
            {
                return default(T);
            }
            return new T().DoTo(t => t.FromRedisHash(entries));
        }

        public void Set(string key, T value)
        {
            this.redisDatabase.HashSet(keyBuilder(key), value.AsRedisHash());
        }
    }
    
    public interface IRedisHashable  {
        HashEntry[] AsRedisHash();
        void FromRedisHash(HashEntry[] entries);
    }
}