using System;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Skolverket.Kontosynk
{
    class Program
    {
        const string ClientCertificateThumbprint = "34 78 25 aa 95 c6 d2 f3 86 9a 3b fe 40 81 32 f7 40 9e 23 14";

        static void Main(string[] args) => HandleMain().GetAwaiter().GetResult();

        static async Task HandleMain()
        {
            if (!File.Exists("Skolverket.jwk"))
            {
                Console.WriteLine("Skolverket signing key does not exist");
                Environment.Exit(2);
            }

            using (var handler = new MutualTlsHandler())
            {
                using (var store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadOnly);
                    var clientCert = store.Certificates.Find(X509FindType.FindByThumbprint, ClientCertificateThumbprint.Trim(), true);
                    if (clientCert.Count > 0)
                    {
                        handler.ClientCertificates.Add(clientCert[0]);
                        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                        handler.SslProtocols = SslProtocols.Tls12;
                    }
                    else
                    {
                        Console.WriteLine("Failed to find certificate: {0}", ClientCertificateThumbprint);
                        Environment.Exit(3);
                    }
                }

                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync("https://localhost:44379/");
                }
            }
        }
    }
}
