using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    // Based on https://github.com/kirei/tls-fed-auth/blob/master/tls-fed-metadata.yaml
    public class TlsFederationIssuer
    {
        [JsonProperty("x509certificate")]
        public string Certificate { get; private set; }
    }
}