using BallastLaneApplication.Domain.DTOs;
using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Service.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(string id);
        Task CreateUserAsync(User user);
        string? Authenticate(string email, string password);
        bool VerifyPassword(UserDTO dto);
    }
}
