using Dapper;

namespace nx_api.Domain.Context
{
    public interface IContextBase
    {
        Task ExecuteWithTransactionAsync(string procedure, DynamicParameters? parameters = default, int timeout = 30);
    }
}
