using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hb.Domain.Entities.Base
{
    public abstract class Entity : IEntityBase<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Entity Clone()
        {
            return (Entity)this.MemberwiseClone();
        }
    }
}
