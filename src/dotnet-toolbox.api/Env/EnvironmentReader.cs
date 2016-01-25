using Newtonsoft.Json.Linq;

namespace dotnet_toolbox.api.Env
{
    public class EnvironmentReader
    {
        private string env;

        public static EnvironmentReader FromEnvironment()
        {
            return new EnvironmentReader(System.Environment.GetEnvironmentVariable("VCAP_SERVICES"));
        }

        public EnvironmentReader(string vcapServicesValue)
        {
            this.env = vcapServicesValue;
        }

        private string RedisConnectionStringFromEnv()
        {
            var credentials = JObject.Parse(this.env)["p-redis"].First["credentials"];
            return credentials.ToObject<RedisCredential>().ConnectionString;
        }

        public string RedisConnectionString
        {
            get
            {
                return HasEnv() ? RedisConnectionStringFromEnv() : "localhost";
            }
        }

        private bool HasEnv()
        {
            return this.env != null;
        }

        private class RedisCredential
        {
            public string Host { get; set; }
            public string Password { get; set; }
            public string Port { get; set; }

            public string ConnectionString
            {
                get
                {
                    return string.Format("{0}:{1},password={2}", Host, Port, Password);
                }
            }
        }
    }
}