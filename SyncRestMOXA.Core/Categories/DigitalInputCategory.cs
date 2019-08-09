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
    public class DigitalInputCategory
        : IDigitalInputCategory
    {
        readonly string _requestUrl = "/api/slot/0/io/di";
        readonly ISyncRestMOXAContext _mxioContext;

        readonly object _timerLocker;

        readonly Timer _observableTimer;
        DigitalInputCollection _lastdigitalInputs;

        public DigitalInputCategory(ISyncRestMOXAContext mxioContext)
        {
            _timerLocker = new object();
            _mxioContext = mxioContext;
           // _observableTimer = new Timer(async (_) => await ObservableStateAsync(_), null, 1000, 1000);
        }

        public event EventHandler<DigitalInputCollection> OnChanged;

        public DigitalInputCollection Get()
        {
            var request = new RestRequest(_requestUrl) { Timeout = 2500 };

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = _mxioContext.Client.Execute(request);

            var parsedObject = JObject.Parse(response.Content);
            var collectionJson = parsedObject["io"]["di"].ToString();

            var diCollection = JsonConvert.DeserializeObject<DigitalInputCollection>(collectionJson);

            return diCollection;
        }

        public async Task<DigitalInputCollection> GetAsync()
        {
            var request = new RestRequest(_requestUrl) { Timeout = 2500 };

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = await _mxioContext.Client.ExecuteTaskAsync(request);

            var parsedObject = JObject.Parse(response.Content);
            var collectionJson = parsedObject["io"]["di"].ToString();

            var diCollection = JsonConvert.DeserializeObject<DigitalInputCollection>(collectionJson);

            return diCollection;
        }

        public void Put(DigitalInputCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(RelayCollection), "DigitalInputCollection was nullable");

            var request = new RestRequest(_requestUrl, Method.PUT) { Timeout = 2500 };

            var jsonRelayCollection = JsonConvert.SerializeObject(collection);

            var jsonParts = this.ConstructJsonBodyParts(jsonRelayCollection);

            request.AddJsonBody(jsonParts);

            _mxioContext.MandatoryHeaders.Select(t => new { t.Key, t.Value }).ToList().ForEach(t => request.AddHeader(t.Key, t.Value));

            IRestResponse response = _mxioContext.Client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Put query was failder with {response.StatusCode}");
        }

        public async Task PutAsync(DigitalInputCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(RelayCollection), "DigitalInputCollection was nullable");

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

                DigitalInputCollection digitalInputs = default;

                try
                {
                    digitalInputs = await this.GetAsync();
                }
                finally { }

                
                if (_lastdigitalInputs == null)
                {
                    _lastdigitalInputs = digitalInputs;
                    return;
                }

                var differentDigitalInputs = digitalInputs.Where(t => _lastdigitalInputs.FirstOrDefault(k => k.Index == t.Index).Status != t.Status);

                if (differentDigitalInputs.Count() != 0)
                    OnChanged?.Invoke(this, digitalInputs);

                _lastdigitalInputs = digitalInputs;
            });
        }

        private object ConstructJsonBodyParts(string jsonRelayCollection)
            => "{ \"slot\": 0, \"io\" : { \"di\":" + jsonRelayCollection + "} }";
    }
}
