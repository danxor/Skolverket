using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationEndpoint
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("base_uri")]
        public string BaseUri { get; set; }

        [JsonProperty("pins")]
        public TlsFederationPin[] Pins {get;set;}
    }
}