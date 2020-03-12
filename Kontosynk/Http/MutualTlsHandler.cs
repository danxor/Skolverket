using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skolverket.Kontosynk
{
    public sealed class MutualTlsHandler : HttpClientHandler
    {
        private static readonly HttpClient _client = new HttpClient();

        private DateTime _cacheInvalidationTime = new DateTime(1970, 1, 1);
        private TlsFederationMetadata _metadata;
        private readonly Uri _metadataUrl;

        public MutualTlsHandler()
            : this(new Uri("https://md.swefed.se/kontosynk/kontosynk-prod-1.jws"))
        {
        }

        public MutualTlsHandler(Uri metadataUrl)
        {
            _metadataUrl = metadataUrl;
            ServerCertificateCustomValidationCallback = OnValidateServerCertificate;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }

        private bool OnValidateServerCertificate(HttpRequestMessage request, X509Certificate2 serverCertificate, X509Chain certificateChain, SslPolicyErrors policyErrors)
        {
            EnsureMetadata();

            var entity = FindEntityByCertificate(ref serverCertificate);
            if (entity != null)
            {
                return true;
            }

            return false;
        }

        private TlsFederationEntity FindEntityByCertificate(ref X509Certificate2 serverCertificate)
        {
            // NOTE: If pins ever change to allow something other than SHA256 this should need to be changed
            var hash = serverCertificate.GetCertHash(HashAlgorithmName.SHA256);
            var hashString = Convert.ToBase64String(hash);

            // TODO: Refactor to LINQ query
            foreach (var entity in _metadata.Entities)
            {
                if (entity.Servers != null)
                {
                    foreach (var server in entity.Servers)
                    {
                        foreach (var pin in server.Pins)
                        {
                            if (pin.Value.Equals(hashString))
                            {
                                foreach (var issuer in entity.Issuers)
                                {
                                    var cert = new X509Certificate2(Encoding.ASCII.GetBytes(issuer.Certificate));
                                    if (cert.Equals(serverCertificate))
                                    {
                                        return entity;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private void EnsureMetadata()
        {
            lock (this)
            {
                var startTime = DateTime.Now;

                if (startTime > _cacheInvalidationTime)
                {
                    var token = _client.GetStringAsync(_metadataUrl).GetAwaiter().GetResult();
                    _metadata = JwsHelper.Load(token);
                    _cacheInvalidationTime = startTime.AddSeconds(3600.0);
                }
            }
        }
    }
}