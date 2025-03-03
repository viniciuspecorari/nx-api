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
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository repository, ILogger<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
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

        public async Task DeleteUser(string id) => await _repository.DeleteUser(id);        
        public async Task<User> GetUserById(string id) => await _repository.GetUserById(id);
        public async Task<IEnumerable<User>> GetUsers() => await _repository.GetUsers();

        public async Task UpdateUser(UserDto userDto)
        {            
            // Update Password
            if (userDto.Password is not null && userDto.NewPassword is not null)
            {
                try
                {
                    var user = GetUserById(userDto.Id);

                    if (!VerifyPasswordHash(userDto.Password, user.Result.Password, user.Result.Salt))
                        throw new Exception("A senha informada é inválida!");

                    // CRIANDO SENHA
                    CreatePasswordHash(userDto.NewPassword,
                                        out string passwordHash,
                                        out string passwordSalt);

                    userDto.NewPassword = passwordHash;
                    userDto.NewSalt = passwordSalt;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar os dados do usuário {id}. A senha informada é inválida!", userDto.Id);
                    throw;
                }               
            }

            await _repository.UpdateUser(userDto);
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
