using System;
using System.Text.Json.Serialization;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationMetadata
    {
        [JsonPropertyName("entities")]
        public TlsFederationEntity[] Entities { get; set; }
    }
}
