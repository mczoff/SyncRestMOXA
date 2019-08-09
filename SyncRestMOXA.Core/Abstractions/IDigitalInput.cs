using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Abstractions
{
    public interface IDigitalInput
    {
        int Index { get; set; }
        int Mode { get; set; }
        int Status { get; set; }
    }
}
