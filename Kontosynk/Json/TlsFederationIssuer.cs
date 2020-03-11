using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    public class TlsFederationIssuer
    {
        [JsonProperty("x509certificate")]
        public string Certificate { get; private set; }
    }
}