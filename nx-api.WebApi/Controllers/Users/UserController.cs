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
        public async Task<IActionResult> CreateUser([FromBody] UserDto request)
        {
            await _service.CreateUser(request);
            return Ok();
        }

        [Route("GetUserById")]
        [HttpGet]
        public async Task<IActionResult> GetUserById([FromQuery] string id)
        {            
            return Ok(await _service.GetUserById(id));
        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _service.GetUsers());
        }

        [Route("UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto? userDto)
        {
            await _service.UpdateUser(userDto);
            return Ok();
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            await _service.DeleteUser(id);
            return Ok();
        }
    }
}
