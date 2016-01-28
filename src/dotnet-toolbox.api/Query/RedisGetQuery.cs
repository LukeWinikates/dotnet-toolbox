using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public class RedisGetQuery<T> : IGetQuerier<T>
    {
        private IDatabase redisDatabase;
        private Func<string, RedisKey> keyBuilder;

        public RedisGetQuery(IDatabase redisDatabase, Func<string, RedisKey> keyBuilder)
        {
            this.redisDatabase = redisDatabase;
            this.keyBuilder = keyBuilder;
        }

        public T Get(string key)
        {
            return JsonConvert.DeserializeObject<T>(redisDatabase.StringGet(keyBuilder(key)));
        }
    }
}