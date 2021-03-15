using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var disco = await client
                .GetDiscoveryDocumentAsync("https://localhost:6003");
            if (disco.IsError)
            {
                CacheError(disco.Error);
            }

            var tokenResponse = await client
                .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1"
                });

            if (tokenResponse.IsError)
                CacheError(tokenResponse.Error);

            Console.WriteLine("=========================================");
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("=========================================");

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient
                .GetAsync("https://localhost:6002/api/v1/product/1");
            if (!response.IsSuccessStatusCode)
                CacheError(response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("*******************************************");
            Console.WriteLine(JsonConvert.SerializeObject(content));
            Console.WriteLine("*******************************************");

            Console.ReadKey();
        }

        private static void CacheError(object error)
        {
            Console.WriteLine($"Error: {error}!");
            Console.WriteLine("Press any key to exit");
            return;
        }
    }
}
