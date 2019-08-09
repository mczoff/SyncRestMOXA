using SyncRestMOXA.Factories;
using SyncRestMOXA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var starupConfig = SyncRestMOXAStartupConfigFactory.Create(args.ElementAtOrDefault(1));

            SyncMOXAService syncMOXAService = new SyncMOXAService();

        }
    }
}
