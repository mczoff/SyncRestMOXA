using Newtonsoft.Json;
using SyncRestMOXA.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Models
{
    public class Relay
        : IRelay
    {
        [JsonProperty("relayIndex")]
        public int Index { get; set; }

        [JsonProperty("relayMode")]
        public int Mode { get; set; }

        [JsonProperty("relayStatus")]
        public int Status { get; set; }

        [JsonProperty("relayTotalCount")]
        public long TotalCount { get; set; }

        [JsonProperty("relayCurrentCount")]
        public long CurrentCount { get; set; }

        [JsonProperty("relayCurrentCountReset")]
        public long CurrentCountReset { get; set; }
    }
}
