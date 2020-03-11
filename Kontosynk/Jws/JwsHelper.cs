using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Skolverket.Kontosynk
{
    public static class JwsHelper
    {
        public static TlsFederationMetadata Load(TextReader stream)
        {
            var serializer = new JsonSerializer();

            using (var reader = new JsonTextReader(stream))
            {
                var body = serializer.Deserialize<JwsBody>(reader);

                return Validate(body);
            }
        }

        public static TlsFederationMetadata Load(string content)
        {
            var body = JsonConvert.DeserializeObject<JwsBody>(content);

            return Validate(body);
        }

        private static TlsFederationMetadata Validate(JwsBody body)
        {
            foreach (var signature in body.Signatures)
            {
                var key = JsonWebKey.Create(File.ReadAllText("Skolverket.jwk"));
                var handler = new JsonWebTokenHandler();

                var jwsCompact = signature.Protected + "." + body.Payload + "." + signature.Signature;

                var validation = new TokenValidationParameters()
                {
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuer = false
                };

                var tokenValidation = handler.ValidateToken(jwsCompact, validation);

                if (tokenValidation.IsValid)
                {
                    var base64 = body.Payload.PadRight(body.Payload.Length + (4 - body.Payload.Length % 4) % 4, '=');

                    var content = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

                    return JsonConvert.DeserializeObject<TlsFederationMetadata>(content);
                }
                else
                {
                    return null;
                }
            }
 
            return null;
        }

        internal class JwsSignature
        {
            [JsonProperty("signature")]
            internal string Signature { get; set; }

            [JsonProperty("protected")]
            internal string Protected { get; set; }
        }

        internal class JwsBody
        {
            [JsonProperty("payload")]
            internal string Payload { get; set; }

            [JsonProperty("signatures")]
            internal JwsSignature[] Signatures { get; set; }
        }
    }
}
