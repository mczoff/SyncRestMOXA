using SyncRestMOXA.Core.Abstractions;
using SyncRestMOXA.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Collections
{
    public class DigitalInputCollection
        : Collection<DigitalInput>
    {
        public DigitalInputCollection(IList<DigitalInput> list)
           : base(list)
        {
        }
    }
}
