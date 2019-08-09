using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SyncRestMOXA.Core.Abstractions;
using SyncRestMOXA.Core.Collections;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Categories
{
    public class RelayCategory
        : IRelayCategory
    {
        readonly string _requestUrl = "/api/slot/0/io/relay";
        readonly ISyncRestMOXAContext _mxioContext;
        readonly Timer _observableTimer;
        readonly object _timerLocker;

        RelayCollection _lastRelays;
        
        public RelayCategory(ISyncRestMOXAContext mxioContext)
        {
            _timerLocker = new object();
            _mxioContext = mxioContext;
            _observableTimer = new Timer(async (_) => await ObservableStateAsync(_), null, 1000, 350);
        }

        public event EventHandler<RelayCollection> OnChanged;

        public RelayCollection Get()
        {
            var request = new RestRequest(_requestUrl) { Timeout = 2500 };

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = _mxioContext.Client.Execute(request);

            if (string.IsNullOrEmpty(response.Content))
                throw new Exception("Null response");

            var parsedObject = JObject.Parse(response.Content);
            var collectionJson = parsedObject["io"]["relay"].ToString();

            var relayCollection = JsonConvert.DeserializeObject<RelayCollection>(collectionJson);

            return relayCollection;
        }

        public async Task<RelayCollection> GetAsync()
        {
            var request = new RestRequest(_requestUrl) { Timeout = 2500 };

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = await _mxioContext.Client.ExecuteTaskAsync(request);

            if (string.IsNullOrEmpty(response.Content))
                throw new Exception("Null response");

            var parsedObject = JObject.Parse(response.Content);
            var collectionJson = parsedObject["io"]["relay"].ToString();

            var relayCollection = JsonConvert.DeserializeObject<RelayCollection>(collectionJson);

            return relayCollection;
        }

        public void Put(RelayCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(RelayCollection), "RelayCollection was nullable");

            var request = new RestRequest(_requestUrl, Method.PUT) { Timeout = 2500 };

            var jsonRelayCollection = JsonConvert.SerializeObject(collection);

            var jsonParts = this.ConstructJsonBodyParts(jsonRelayCollection);

            request.AddJsonBody(jsonParts);

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = _mxioContext.Client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Put query was failder with {response.StatusCode}");
        }

        public async Task PutAsync(RelayCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(RelayCollection), "RelayCollection was nullable");

            var request = new RestRequest(_requestUrl, Method.PUT) { Timeout = 2500 };

            var jsonRelayCollection = JsonConvert.SerializeObject(collection);

            var jsonParts = this.ConstructJsonBodyParts(jsonRelayCollection);

            request.AddJsonBody(jsonParts);

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = await _mxioContext.Client.ExecuteTaskAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Put query was failder with {response.StatusCode}");
        }

        private async Task ObservableStateAsync(object state)
        {
            await Task.Run(async () =>
            {
                Ping ping = new Ping();
                PingReply reply = await ping.SendPingAsync(IPAddress.Parse(_mxioContext.Client.BaseUrl.Host));

                if (reply.Status != IPStatus.Success)
                    return;

                RelayCollection relayInputs = default;

                //TODO:Include 
                try
                {
                    relayInputs = await this.GetAsync();
                }
                finally { }

                if (_lastRelays == null)
                {
                    _lastRelays = relayInputs;
                    return;
                }

                var differentDigitalInputs = relayInputs.Where(t => _lastRelays.FirstOrDefault(k => k.Index == t.Index).Status != t.Status);

                if (differentDigitalInputs.Count() != 0)
                    OnChanged?.Invoke(this, relayInputs);


                _lastRelays = relayInputs;
            });
        }

        private object ConstructJsonBodyParts(string jsonRelayCollection)
            => "{ \"slot\": 0, \"io\" : { \"relay\":" + jsonRelayCollection + "} }";
    }
}
