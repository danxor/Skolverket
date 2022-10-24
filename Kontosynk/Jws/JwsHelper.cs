using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skolverket.Kontosynk
{
    public static class JwsHelper
    {
        public static TlsFederationMetadata Load(Stream stream)
        {
            var body = JsonSerializer.Deserialize<JwsBody>(stream);

            return Validate(body);
        }

        public static TlsFederationMetadata Load(string content)
        {
            var body = System.Text.Json.JsonSerializer.Deserialize<JwsBody>(content);
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

                    return JsonSerializer.Deserialize<TlsFederationMetadata>(content);
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
            [JsonPropertyName("signature")]
            internal string Signature { get; set; }

            [JsonPropertyName("protected")]
            internal string Protected { get; set; }
        }

        internal class JwsBody
        {
            [JsonPropertyName("payload")]
            internal string Payload { get; set; }

            [JsonPropertyName("signatures")]
            internal JwsSignature[] Signatures { get; set; }
        }
    }
}
