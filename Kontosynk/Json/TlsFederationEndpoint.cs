using System;
using System.Text.Json.Serialization;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationEndpoint
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }

        [JsonPropertyName("base_uri")]
        public string BaseUri { get; set; }

        [JsonPropertyName("pins")]
        public TlsFederationPin[] Pins {get;set;}
    }
}