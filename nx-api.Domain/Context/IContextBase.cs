using Dapper;

namespace nx_api.Domain.Context
{
    public interface IContextBase
    {
        Task ExecuteWithTransactionAsync(string procedure, DynamicParameters? parameters = default, int timeout = 30);
        Task<IEnumerable<dynamic>> GetCollectionAsync<dynamic>(string procedure, DynamicParameters? parameters = default, int timeout = 30);
        Task<dynamic> GetAsync<dynamic>(string procedure, DynamicParameters? parameters = default, int timeout = 30);
    }
}
