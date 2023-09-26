using BallastLaneApplication.Data.Context;
using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Domain.Entities;
using MongoDB.Driver;

namespace BallastLaneApplication.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IProductContext _productContext;

        public UserRepository(IProductContext productContext)
        {
            _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _productContext.Users.Find(x => true).ToListAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _productContext.Users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAndPassword(string email, string password)
        {
            return await _productContext.Users.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user)
        {
            await _productContext.Users.InsertOneAsync(user);
        }
    }
}
