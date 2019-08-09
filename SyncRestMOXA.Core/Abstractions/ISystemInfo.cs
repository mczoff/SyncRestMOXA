using SyncRestMOXA.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Abstractions
{
    public interface ISystemInfo
    {
        Device[] Device { get; set; }
        Network Network { get; set; }
    }
}
