using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace dotnet_toolbox.api.Query
{
    public class RedisGetQuery<T> : IGetQuerier<T> where T : class
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
            var objectJson = redisDatabase.StringGet(keyBuilder(key));
            if(objectJson.IsNull) {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(objectJson);
        }
    }
}