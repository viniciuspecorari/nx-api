using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;

namespace nx_api.Domain.Repositories.Users
{
    public interface IUserRepository
    {
        Task CreateUser(User create);
        Task UpdateUser(UserDto update);
        Task DeleteUser(string id);
        Task<User> GetUserById(string id);
        Task<IEnumerable<User>> GetUsers();
    }
}
