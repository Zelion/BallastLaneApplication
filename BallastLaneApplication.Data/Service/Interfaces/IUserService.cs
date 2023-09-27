using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;
using BallastLaneApplication.Domain.Enums;

namespace BallastLaneApplication.Data.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<UserCreationResults> CreateUserAsync(User user);
        string? Authenticate(string email, string password);
        bool VerifyPassword(UserDTO dto);
    }
}
