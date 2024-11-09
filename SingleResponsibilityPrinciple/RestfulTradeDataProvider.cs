using SingleResponsibilityPrinciple.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple
{
    public class RestfulTradeDataProvider : ITradeDataProvider
    {
        private readonly string url;
        private readonly ILogger logger;
        private readonly HttpClient client = new HttpClient();

        public RestfulTradeDataProvider(string url, ILogger logger)
        {
            this.url = url;
            this.logger = logger;
        }

        public async IAsyncEnumerable<string> GetTradeDataAsync()
        {
            logger.LogInfo("Connecting to the Restful server using HTTP");

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<string> tradesString = JsonSerializer.Deserialize<List<string>>(content);
                if (tradesString != null)
                {
                    logger.LogInfo("Received trade strings of length = " + tradesString.Count);
                    foreach (var trade in tradesString)
                    {
                        yield return trade;
                    }
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
