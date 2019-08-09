using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Abstractions
{
    public interface IRelay
    {
        int Index { get; set; }
        int Mode { get; set; }
        int Status { get; set; }
        long TotalCount { get; set; }
        long CurrentCount { get; set; }
        long CurrentCountReset { get; set; }
    }
}
