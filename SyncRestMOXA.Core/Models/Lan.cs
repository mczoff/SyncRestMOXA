using SyncRestMOXA.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Models
{
    public class Lan
        : ILan
    {
        public string LanMac { get; set; }
        public string LanIp { get; set; }
    }
}
