using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_toolbox.common
{
    public static class Constants
    {
        public static class Redis
        {
            public static readonly string PackageCrawlerJobQueueName = "PackageCrawlerJob";
            public static readonly int PACKAGES_DB = 1;
        }
    }
}
