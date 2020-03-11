using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    public sealed class TlsFederationPin
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}