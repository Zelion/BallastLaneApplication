using AutoMapper;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BallastLaneAuth.Controllers
{
    [Authorize]
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
            _service = service ?? throw new ArgumentNullException(nameof(_service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync(UserDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(dto);
            var userCreationResult = await _service.CreateUserAsync(user);
            if (userCreationResult == UserCreationResults.EmailAlreadyTaken)
            {
                return BadRequest("Email already taken");
            }

            return Ok(dto);
        }

        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public ActionResult Login([FromBody] UserDTO dto)
        {
            var verifiedPassword = _service.VerifyPassword(dto);
            if (!verifiedPassword)
                return BadRequest();

            var token = _service.Authenticate(dto.Email, dto.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token, dto });
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _service.GetUsersAsync();
            if (!users.Any())
            {
                return NotFound();
            }

            var dtos = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _service.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserDTO>(user);

            return Ok(dto);
        }
    }
}
