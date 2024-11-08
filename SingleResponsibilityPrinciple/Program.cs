using System;
using SingleResponsibilityPrinciple.AdoNet;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new ConsoleLogger();

            // URL to read trade file from
            string tradeURL = "http://raw.githubusercontent.com/tgibbons-css/CIS3285_Unit9_F24/refs/heads/master/SingleResponsibilityPrinciple/trades.txt";

            ITradeValidator tradeValidator = new SimpleTradeValidator(logger);

            // Initialize URLTradeDataProvider (note capitalization)
            ITradeDataProvider urlProvider = new URLTradeDataProvider(tradeURL, logger);

            // Wrap with AdjustTradeDataProvider to replace GBP with EUR
            ITradeDataProvider adjustedProvider = new AdjustTradeDataProvider(urlProvider);

            // Wrap with URLAsyncProvider for asynchronous reading
            ITradeDataProvider asyncProvider = new URLAsyncProvider(adjustedProvider);

            ITradeMapper tradeMapper = new SimpleTradeMapper();
            ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
            ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

            // Use asyncProvider in TradeProcessor to read asynchronously with currency conversion
            TradeProcessor tradeProcessor = new TradeProcessor(asyncProvider, tradeParser, tradeStorage);
            tradeProcessor.ProcessTrades();
        }
    }
}
