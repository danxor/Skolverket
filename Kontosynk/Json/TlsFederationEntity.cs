using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public sealed class TlsFederationEntity
    {
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        [JsonProperty("organization", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Organization { get; set; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }

        [JsonProperty("issuers")]
        public TlsFederationIssuer[] Issuers { get; set; }

        [JsonProperty("servers")]
        public TlsFederationEndpoint[] Servers { get; set; }

        [JsonProperty("clients")]
        public TlsFederationEndpoint[] Clients { get; set; }
    }
}
