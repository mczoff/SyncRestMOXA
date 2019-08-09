using Newtonsoft.Json;
using SyncRestMOXA.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Collections
{
    public class RelayCollection
        : Collection<Relay>
    {
        public RelayCollection(IList<Relay> list) 
            : base(list)
        {
        }
    }
}
