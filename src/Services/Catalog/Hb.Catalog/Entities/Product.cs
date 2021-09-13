using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hb.Catalog.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //[BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonElement("Name")]
        //[BsonRequired]
        public string Name { get; set; }
        public string Description { get; set; }
        //[BsonRequired]
        public decimal Price { get; set; }
        //[BsonRequired]
        public string Currency { get; set; }
    }
}
