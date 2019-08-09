using SyncRestMOXA.Core.Abstractions;
using SyncRestMOXA.Core.Categories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core
{
    public class SyncRestMOXAContext
        : ISyncRestMOXAContext
    {
        public IRestClient Client { get; set; }

        public ISystemInfoCategory SystemInfoCategory { get; set; }
        public IDigitalInputCategory DigitalInputCategory { get; set; }
        public IRelayCategory RelayCategory { get; set; }

        public Dictionary<string, string> MandatoryHeaders { get; private set; } = new Dictionary<string, string>()
        {
            { "Content-Type", "application/json" },
            { "Accept", "vdn.dac.v1"}
        };

        public SyncRestMOXAContext (SyncRestMOXAContextStartupParams mxioContextStartupParams)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            Client = new RestClient(mxioContextStartupParams.Host);

            SystemInfoCategory = new SystemInfoCategory(this);
            DigitalInputCategory = new DigitalInputCategory(this);
            RelayCategory = new RelayCategory(this);
        }
    }
}
