using Microsoft.Extensions.Logging;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;
using nx_api.Domain.Repositories.Users;
using nx_api.Domain.Services.Users;
using System.Security.Cryptography;

namespace nx_api.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;        

        public UserService(IUserRepository repository)
        {
            _repository = repository;            
        }

        #region [UserMethods]
        public async Task CreateUser(UserDto create)
        {            

            // CRIANDO SENHA
            CreatePasswordHash(create.Password,
                                out string passwordHash,
                                out string passwordSalt);

            var newUser = new User
            {
                Name = create.Name,
                Email = create.Email,
                Password = passwordHash,
                Salt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
            };


            await _repository.CreateUser(newUser);
        }

        public Task DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _repository.GetUserById(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _repository.GetUsers();
        }

        public Task UpdateUser(string id, UserDto update)
        {
            throw new NotImplementedException();
        }
        #endregion



        #region [Utils]
        private static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
        }

        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
    }
    #endregion
}
