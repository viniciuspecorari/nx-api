using Dapper;
using Microsoft.Extensions.Logging;
using nx_api.Domain.Context;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;
using nx_api.Domain.Repositories.Users;
using System.Drawing;
using System.Text.Json;

namespace nx_api.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly INXDBContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(INXDBContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateUser(UserDto create)
        {
            try
            {
                string procedure = "[ONBOARDING].[USERS_INS]";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@NAME", create.Name);
                parameters.Add("@EMAIL", create.Email);
                parameters.Add("@PASSWORD", create.Password);

                await _context.ExecuteWithTransactionAsync(procedure, parameters, 60);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Houve um problema ao cadastrar o usuário: {UserJson}", JsonSerializer.Serialize(create));
                throw;
            }
        }

        public Task DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(string id, UserDto update)
        {
            throw new NotImplementedException();
        }
    }
}
