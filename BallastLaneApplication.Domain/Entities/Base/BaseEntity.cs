using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BallastLaneApplication.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
