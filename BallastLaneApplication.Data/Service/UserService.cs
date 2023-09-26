using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BallastLaneApplication.Data.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string key;

        public UserService(
            IConfiguration configuration,
            IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
            key = configuration.GetSection("JwtKey").ToString();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _userRepository.GetUserAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public string? Authenticate(string email, string password)
        {
            var user = _userRepository.GetByUsernameAndPassword(email, password);
            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
