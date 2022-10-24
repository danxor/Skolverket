using System;
using System.Text.Json.Serialization;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public class TlsFederationIssuer
    {
        [JsonPropertyName("x509certificate")]
        public string Certificate { get; private set; }
    }
}