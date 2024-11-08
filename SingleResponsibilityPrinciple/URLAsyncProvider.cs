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

    public IEnumerable<string> GetTradeData()
    {
        // Run the base provider's GetTradeData asynchronously
        Task<IEnumerable<string>> task = Task.Run(() => _baseProvider.GetTradeData());

        // Wait for the task to complete and return the result
        return task.Result;
    }
}
