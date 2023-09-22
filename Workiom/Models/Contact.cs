using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Workiom.Models
{
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("companies")]
        public List<string> CompanyIds { get; set; }

        [BsonElement("extraFields")]
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
