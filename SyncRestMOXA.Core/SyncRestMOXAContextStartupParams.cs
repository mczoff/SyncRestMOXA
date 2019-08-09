using SyncRestMOXA.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core
{
    public class SyncRestMOXAContextStartupParams
    {
        public string Host { get; set; }
        public ModuleMXIOType ModuleType { get; set; }
    }
}
