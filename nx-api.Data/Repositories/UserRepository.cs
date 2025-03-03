using Dapper;
using Microsoft.Extensions.Logging;
using nx_api.Domain.Context;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;
using nx_api.Domain.Repositories.Users;
using System.Data;
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

        public async Task CreateUser(User create)
        {            
            try
            {
                _logger.LogInformation("Creating user...");

                string procedure = "[ONBOARDING].[USERS_INS]";                

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@NAME", create.Name);
                parameters.Add("@EMAIL", create.Email);
                parameters.Add("@PASSWORD", create.Password);
                parameters.Add("@SALT", create.Salt);

                await _context.ExecuteWithTransactionAsync(procedure, parameters, 60);

                _logger.LogInformation("User created successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocurred create user: {UserJson}", JsonSerializer.Serialize(create));
                throw;
            }
        }

        public async Task DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation("Deletando usuário {id}...", id);
                string procedure = "[ONBOARDING].[USER_DEL]";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ID", id);

                 await _context.ExecuteWithTransactionAsync(procedure, parameters, 60);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocurred delete user: {id}", id);
                throw;
            }
        }

        public async Task<User> GetUserById(string id)
        {
            try
            {
                _logger.LogInformation("Buscando usuário...");
                string procedure = "[ONBOARDING].[USER_BY_ID_GET]";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@USER_ID", id);

                return await _context.GetAsync<User>(procedure, parameters, 60);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocurred get user: {id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Buscando usuários...");
                string procedure = "[ONBOARDING].[USERS_GET]";

                return await _context.GetCollectionAsync<User>(procedure, null, 60);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocurred get users");
                throw;
            }
        }

        public async Task UpdateUser(UserDto userDto)
        {
            try
            {
                _logger.LogInformation("Atualizando dados do usuário {id}...", userDto.Id);
                string procedure = "[ONBOARDING].[USERS_UPT]";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ID", userDto.Id);
                parameters.Add("@NAME", userDto.Name);
                parameters.Add("@EMAIL", userDto.Email);
                parameters.Add("@PASSWORD", userDto.NewPassword);
                parameters.Add("@SALT", userDto.NewSalt);

                await _context.ExecuteWithTransactionAsync(procedure, parameters, 60);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ocurred update user");
                throw;
            }
        }
    }
}
