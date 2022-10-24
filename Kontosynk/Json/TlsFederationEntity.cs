using System;
using System.Text.Json.Serialization;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationEntity
    {
        [JsonPropertyName("entity_id")]
        public string EntityId { get; set; }

        [JsonPropertyName("organization")]
        public string Organization { get; set; }

        [JsonPropertyName("organization_id")]
        public string OrganizationId { get; set; }

        [JsonPropertyName("issuers")]
        public TlsFederationIssuer[] Issuers { get; set; }

        [JsonPropertyName("servers")]
        public TlsFederationEndpoint[] Servers { get; set; }

        [JsonPropertyName("clients")]
        public TlsFederationEndpoint[] Clients { get; set; }
    }
}
