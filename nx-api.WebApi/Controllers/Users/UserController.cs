using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nx_api.Domain.Dtos.Users;
using nx_api.Domain.Services.Users;

namespace nx_api.WebApi.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;        

        public UserController(IUserService service)
        {
            _service = service;            
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto request)
        {
            await _service.CreateUser(request);
            return Ok();
        }     
    }
}
