using Newtonsoft.Json;
using SyncRestMOXA.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Models
{
    public class SystemInfo
        : ISystemInfo
    {
        public Device[] Device { get; set; }
        public Network Network { get; set; }
    }
}
