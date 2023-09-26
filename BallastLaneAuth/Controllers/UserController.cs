using AutoMapper;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BallastLaneAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(
            IUserService service,
            IMapper mapper
            )
        {
            _service = service;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public ActionResult Login([FromBody] UserDTO dto)
        {
            var token = _service.Authenticate(dto.Email, dto.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token, dto });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _service.GetUsersAsync();
            var dtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _service.GetUserAsync(id);
            var dto = _mapper.Map<UserDTO>(user);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(UserDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            // add validation

            var user = _mapper.Map<User>(dto);
            await _service.CreateUserAsync(user);

            return Ok(user);
        }
    }
}
