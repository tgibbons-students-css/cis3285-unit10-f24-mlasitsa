using System.Collections.Generic;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.Contracts;

public class AdjustTradeDataProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider _baseProvider;

    public AdjustTradeDataProvider(ITradeDataProvider baseProvider)
    {
        _baseProvider = baseProvider;
    }

    public async IAsyncEnumerable<string> GetTradeDataAsync()
    {
        await foreach (var trade in _baseProvider.GetTradeDataAsync())
        {
            yield return trade.Replace("GBP", "EUR");
        }
    }
}
