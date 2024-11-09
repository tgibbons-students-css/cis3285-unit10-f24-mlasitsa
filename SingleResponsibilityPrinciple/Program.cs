using System;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple;
using SingleResponsibilityPrinciple.Contracts;
using SingleResponsibilityPrinciple.AdoNet;


namespace SingleResponsibilityPrinciple
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();

            // URL to read trade file from
            string tradeURL = "http://raw.githubusercontent.com/tgibbons-css/CIS3285_Unit9_F24/refs/heads/master/SingleResponsibilityPrinciple/trades.txt";

            ITradeValidator tradeValidator = new SimpleTradeValidator(logger);

            // Initialize URLTradeDataProvider with async streaming
            ITradeDataProvider urlProvider = new URLTradeDataProvider(tradeURL, logger);

            // Wrap with AdjustTradeDataProvider to replace GBP with EUR
            ITradeDataProvider adjustedProvider = new AdjustTradeDataProvider(urlProvider);

            ITradeMapper tradeMapper = new SimpleTradeMapper();
            ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
            ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

            // Initialize TradeProcessor with the adjusted provider
            TradeProcessor tradeProcessor = new TradeProcessor(adjustedProvider, tradeParser, tradeStorage);

            // Use the asynchronous ProcessTradesAsync method to process trades
            await tradeProcessor.ProcessTradesAsync();
        }
    }
}
