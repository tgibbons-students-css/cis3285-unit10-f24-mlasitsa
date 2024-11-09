using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class URLTradeDataProvider : ITradeDataProvider
    {
        private readonly string url;
        private readonly ILogger logger;
        private readonly HttpClient client;

        public URLTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
            this.client = new HttpClient();
        }

        public async IAsyncEnumerable<string> GetTradeDataAsync()
        {
            logger.LogInfo("Connecting to the Restful server using HTTP");

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using Stream stream = await response.Content.ReadAsStreamAsync();
                using StreamReader reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    yield return await reader.ReadLineAsync();
                }
            }
            else
            {
                logger.LogWarning($"Failed to retrieve data. Status code: {response.StatusCode}");
                throw new Exception($"Error retrieving data from URL: {url}");
            }
        }
    }
}
