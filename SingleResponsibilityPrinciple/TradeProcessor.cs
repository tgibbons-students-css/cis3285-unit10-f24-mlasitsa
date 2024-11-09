using System.Collections.Generic;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class TradeProcessor
    {
        private readonly ITradeDataProvider tradeDataProvider;
        private readonly ITradeParser tradeParser;
        private readonly ITradeStorage tradeStorage;

        public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeParser tradeParser, ITradeStorage tradeStorage)
        {
            this.tradeDataProvider = tradeDataProvider;
            this.tradeParser = tradeParser;
            this.tradeStorage = tradeStorage;
        }

        public async Task ProcessTradesAsync()
        {
            var trades = new List<TradeRecord>();

            // Use await foreach to asynchronously retrieve each trade line
            await foreach (var line in tradeDataProvider.GetTradeDataAsync())
            {
                var parsedTrades = tradeParser.Parse(new List<string> { line });
                trades.AddRange(parsedTrades);
            }

            // Persist all parsed trades
            tradeStorage.Persist(trades);
        }
    }
}
