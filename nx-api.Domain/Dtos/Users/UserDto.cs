namespace nx_api.Domain.Dtos.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Password { get; set; } = null;        
        public string? NewPassword { get; set; } = null;
        public string? NewSalt { get; set; } = null;
    }
}
