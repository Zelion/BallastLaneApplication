using BallastLaneApplication.Domain.Entities.Base;

namespace BallastLaneApplication.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
