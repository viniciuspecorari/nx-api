using Dapper;
using Microsoft.Extensions.Logging;
using nx_api.Domain.Context;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;
using nx_api.Domain.Repositories.Users;
using nx_api.Domain.Services.Users;
namespace nx_api.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }        

        public async Task CreateUser(UserDto create)
        {
            await _repository.CreateUser(create);
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
