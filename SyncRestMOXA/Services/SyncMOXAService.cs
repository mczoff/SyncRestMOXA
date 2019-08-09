using SyncRestMOXA.Core;
using SyncRestMOXA.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncRestMOXA.Services
{
    public class SyncMOXAService
    {
        private ISyncRestMOXAContext[] _urlsSyncDict;
        private int[] _relayIndexes;

        public async Task StartAsync(SyncRestMOXAStartupParams syncRestMOXAStartupParams)
        {
            _relayIndexes = syncRestMOXAStartupParams.RelayIndexes;
            _urlsSyncDict = syncRestMOXAStartupParams.Addresses.Select(t =>
            {
                var context = new SyncRestMOXAContext(new SyncRestMOXAContextStartupParams { Host = t, ModuleType = ModuleMXIOType.E1200 });

                context.RelayCategory.OnChanged += async (sender, args) =>
                {
                    Console.WriteLine($"{context.Client.BaseHost} was changed");

                    var anotherContexts = _urlsSyncDict.Where(k => k.Client.BaseHost != context.Client.BaseHost);

                    anotherContexts.ToList().ForEach(k => Console.WriteLine($"{k.Client.BaseHost} will change"));
                    await Task.WhenAll(anotherContexts.ToList().Select(async k => await k.RelayCategory.PutAsync(args)));
                };

                return context as ISyncRestMOXAContext;
            }).ToArray();
        }

        public async Task StopAsync()
        {
            _urlsSyncDict = null;
        }
    }
}
