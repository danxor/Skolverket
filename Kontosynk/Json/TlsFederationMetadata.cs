using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationMetadata
    {
        [JsonProperty("entities")]
        public TlsFederationEntity[] Entities { get; set; }
    }
}
