using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Bakerscraper.Searchers;

namespace Bakerscraper.Factories
{
    public class BakerscraperHttpClientFactory : IHttpClientFactory
    {
        public const string Cookpad = "Cookpad";

        public HttpClient CreateClient(string name)
        {
            switch (name)
            {
                case Cookpad:
                    var handler = new HttpClientHandler { CookieContainer = new CookieContainer() };
                    var client = new HttpClient(handler)
                    {
                        BaseAddress = new Uri(CookpadRecipeSearch.baseUrl)
                    };
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Bakerscraper-API", "1.0"));
                    client.DefaultRequestHeaders.Host = "cookpad.com";
                    return client;
                default:
                    return new HttpClient();
            }
        }
    }
}
