using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Abstractions
{
    public interface IDevice
    {
        string ModelName { get; set; }
        string DeviceName { get; set; }
        DateTime DeviceUpTime { get; set; }
        string FirmwareVersion { get; set; }
    }
}
