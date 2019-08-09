using SyncRestMOXA.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using SyncRestMOXA.Core.Models;
using Newtonsoft.Json;

namespace SyncRestMOXA.Core.Categories
{
    public class SystemInfoCategory
        : ISystemInfoCategory
    {
        readonly string _requestUrl = "/api/slot/0/sysInfo";
        readonly ISyncRestMOXAContext _mxioContext;

        public SystemInfoCategory(ISyncRestMOXAContext mxioContext)
        {
            _mxioContext = mxioContext;
        }

        public ISystemInfo Get()
        {
            var request = new RestRequest(_requestUrl);

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = _mxioContext.Client.Execute(request);

            var parsedObject = JObject.Parse(response.Content);
            var systemInfoJson = parsedObject["sysInfo"].ToString(); 

            SystemInfo systemInfo = JsonConvert.DeserializeObject<SystemInfo>(systemInfoJson);

            return systemInfo;
        }

        public async Task<ISystemInfo> GetAsync()
        {
            var request = new RestRequest(_requestUrl);

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = await _mxioContext.Client.ExecuteTaskAsync(request);

            var parsedObject = JObject.Parse(response.Content);
            var systemInfoJson = parsedObject["sysInfo"].ToString();

            SystemInfo systemInfo = JsonConvert.DeserializeObject<SystemInfo>(systemInfoJson);

            return systemInfo;
        }
    }
}
