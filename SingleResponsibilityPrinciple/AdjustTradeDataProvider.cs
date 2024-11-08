using System.Collections.Generic;
using SingleResponsibilityPrinciple.Contracts;


public class AdjustTradeDataProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider _baseProvider;

    // Constructor takes an ITradeDataProvider instance, e.g., UrlTradeDataProvider
    public AdjustTradeDataProvider(ITradeDataProvider baseProvider)
    {
        _baseProvider = baseProvider;
    }

    // Implements GetTradeData, replaces "GBP" with "EUR" in each trade
    public IEnumerable<string> GetTradeData()
    {
        // Get trade data from the base provider
        var trades = _baseProvider.GetTradeData();

        // Adjust each trade to replace "GBP" with "EUR"
        foreach (var trade in trades)
        {
            yield return trade.Replace("GBP", "EUR");
        }
    }
}
