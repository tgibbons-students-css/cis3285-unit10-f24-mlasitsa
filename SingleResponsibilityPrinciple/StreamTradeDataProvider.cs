using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class StreamTradeDataProvider : ITradeDataProvider
    {
        private readonly Stream stream;
        private readonly ILogger logger;

        public StreamTradeDataProvider(Stream stream, ILogger logger)
        {
            this.stream = stream;
            this.logger = logger;
        }

        public async IAsyncEnumerable<string> GetTradeDataAsync()
        {
            logger.LogInfo("Reading trades from file stream.");
            using var reader = new StreamReader(stream);

            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                yield return line;
            }
        }
    }
}
