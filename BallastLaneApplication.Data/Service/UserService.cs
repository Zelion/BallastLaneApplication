using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Data.Service.Interfaces;
using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BallastLaneApplication.Data.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly string? key;

        public UserService(
            IConfiguration configuration,
            IUserRepository repository
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));
            key = configuration.GetSection("JwtKey").ToString();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _repository.GetUsersAsync();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            try
            {
                return await _repository.GetUserAsync(id);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }

        public async Task<UserCreationResults> CreateUserAsync(User user)
        {
            try
            {
                var existingUser = await _repository.GetByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return UserCreationResults.EmailAlreadyTaken;
                }

                //hash password
                user.Password = HashPasword(user.Password, out var salt);
                user.Salt = salt;

                SetDefaultValues(user);

                await _repository.CreateUserAsync(user);

                return UserCreationResults.Succeed;
            }
            catch (Exception)
            {
                //TODO: LOG
                return UserCreationResults.Failed;
            }
        }

        public string? Authenticate(string email, string password)
        {
            var user = _repository.GetByEmailAndPasswordAsync(email, password);
            if (user == null || string.IsNullOrEmpty(key))
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

        public bool VerifyPassword(UserDTO user)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(user.Password, user.Salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(user.HashPassword));
        }

        #region Private Methods

        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        private void SetDefaultValues(User user)
        {
            user.Created = DateTime.Now;
            user.CreatedBy = user.Email;
            user.Modified = DateTime.Now;
            user.ModifiedBy = user.Email;
        }

        #endregion
    }
}
