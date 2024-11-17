using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nx_api.Domain.Context;
using System.Data;

namespace nx_api.Data.Context
{
    public abstract class ContextBase : IContextBase
    {

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;        
        protected string ConnectionString { get; private set; }

        public ContextBase(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;            
        }

        public IDbConnection GetConnection()
        {
            if (ConnectionString is null)
                InitializeConnectionString();

            return new SqlConnection(ConnectionString);
        }

        private void InitializeConnectionString()
        {
            ConnectionString =  _configuration.GetConnectionString("NXDB");            
        }

        public async Task ExecuteWithTransactionAsync(string procedure, DynamicParameters? parameters = null, int timeout = 30)
        {
            using var connection = GetConnection();
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            try
            {
                var result = await connection.ExecuteAsync(
                    sql: procedure,
                    param: parameters,
                    transaction: transaction,
                    commandTimeout: timeout,
                    commandType: CommandType.StoredProcedure);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occured while executing {procedure}", procedure);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
