using System.Collections.Generic;
using System.Threading.Tasks;
using SingleResponsibilityPrinciple.Contracts;

public class URLAsyncProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider _baseProvider;

    public URLAsyncProvider(ITradeDataProvider baseProvider)
    {
        _baseProvider = baseProvider;
    }

    public async IAsyncEnumerable<string> GetTradeDataAsync()
    {
        // Use await foreach to get each item asynchronously from the base provider
        await foreach (var trade in _baseProvider.GetTradeDataAsync())
        {
            yield return trade;
        }
    }
}
