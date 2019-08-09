using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Factories
{
    public static class SyncRestMOXAStartupConfigFactory
    {
        public static SyncRestMOXAStartupParams Create(string path)
        {
            if (path != null)
                return JsonConvert.DeserializeObject<SyncRestMOXAStartupParams>(path);

            return JsonConvert.DeserializeObject<SyncRestMOXAStartupParams>("startupConfig.json");
        }
    }
}
