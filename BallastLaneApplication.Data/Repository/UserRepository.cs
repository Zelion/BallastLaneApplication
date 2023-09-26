using BallastLaneApplication.Data.Repository.Interfaces;
using BallastLaneApplication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BallastLaneApplication.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> users;

        public UserRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("DabaseSettings:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("DabaseSettings:DatabaseName").Value);
            users = database.GetCollection<User>(configuration.GetSection("DabaseSettings:CollectionName").Value);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await users.Find(x => true).ToListAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAndPassword(string email, string password)
        {
            return await users.Find(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await users.InsertOneAsync(user);

            return user;
        }
    }
}
