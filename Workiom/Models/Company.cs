using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Workiom.Models
{
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("numberOfEmployees")]
        [BsonDefaultValue(0)]
        public int NumberOfEmployees { get; set; }

        [BsonElement("extraFields")]
        public Dictionary<string, string>? ExtraFields { get; set; }
    }
}
