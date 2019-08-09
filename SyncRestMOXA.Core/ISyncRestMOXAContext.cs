using SyncRestMOXA.Core.Abstractions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core
{
    public interface ISyncRestMOXAContext
    {
        IRestClient Client { get; set; }
        Dictionary<string, string> MandatoryHeaders { get; }

        ISystemInfoCategory SystemInfoCategory { get; set; }
        IDigitalInputCategory DigitalInputCategory { get; set; }
        IRelayCategory RelayCategory { get; set; }
    }
}

