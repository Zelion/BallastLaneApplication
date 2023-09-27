using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailAndPasswordAsync(string username, string password);
        Task CreateUserAsync(User user);
    }
}
