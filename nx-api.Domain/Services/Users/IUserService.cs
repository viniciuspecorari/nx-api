using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;

namespace nx_api.Domain.Services.Users
{
    public interface IUserService
    {
        Task CreateUser(UserDto create);
        Task UpdateUser(string id, UserDto update);
        Task DeleteUser(string id);
        Task<UserDto> GetUser(string id);
    }
}
