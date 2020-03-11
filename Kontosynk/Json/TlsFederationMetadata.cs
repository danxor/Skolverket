using Newtonsoft.Json;
using System;

namespace Skolverket.Kontosynk
{
    public sealed class TlsFederationMetadata
    {
        [JsonProperty("entities")]
        public TlsFederationEntity[] Entities { get; set; }
    }
}
