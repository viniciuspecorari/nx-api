using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;

namespace nx_api.Domain.Repositories.Users
{
    public interface IUserRepository
    {
        Task CreateUser(UserDto create);
        Task UpdateUser(string id, UserDto update);
        Task DeleteUser(string id);
        Task<UserDto> GetUser(string id);
    }
}
