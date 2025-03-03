using Microsoft.Extensions.Logging;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Entities.Users;
using nx_api.Domain.Repositories.Users;
using nx_api.Domain.Services.Users;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using nx_api.WebApi.MiddlewareExceptions;

namespace nx_api.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repository;
        private readonly IConfiguration _config;
        private readonly int _iteration = 3;
        private readonly string _pepper;


        public UserService(IUserRepository repository, ILogger<UserService> logger, IConfiguration config)
        {
            _pepper = config["Hash:pepper"] ?? "";
            _repository = repository;
            _logger = logger;
            _config = config;
        }

        #region [UserMethods]
        public async Task CreateUser(UserDto create)
        {
            var salt = GenerateSalt();
            var newUser = new User
            {
                Name = create.Name,
                Email = create.Email,
                Password = GeneratePasswordHash(create.Password, salt, _pepper, _iteration),
                Salt = salt,
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
                    var user = await GetUserById(userDto.Id);

                    if (VerifyPassword(userDto.Password, user.Password))
                        throw new CustomException("A senha informada é inválida!");

                    var salt = GenerateSalt();
                    userDto.NewPassword = GeneratePasswordHash(userDto.NewPassword, salt, _pepper, _iteration);
                    userDto.NewSalt = salt;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar os dados do usuário {id}. A senha informada é inválida!", userDto.Id);
                    throw;
                }
            }

            await _repository.UpdateUser(userDto);
        }

        public static string GeneratePasswordHash(string password, string salt, string pepper, int iteration)
        {
            if (iteration <= 0) return password;
            using var sha256 = SHA256.Create();
            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return GeneratePasswordHash(hash, salt, pepper, iteration - 1);
        }

        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }

        public bool VerifyPassword(string password, string passwordSave) => passwordSave != password;

        #endregion
    }
}
