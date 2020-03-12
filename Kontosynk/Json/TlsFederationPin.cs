using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationPin
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}