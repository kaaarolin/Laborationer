using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laboration_3_databaser
{
    public class Lagersaldo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Använder ObjectId som primärnyckel
        public ObjectId Id { get; set; } // Skapar nytt ObjectId istället för string

        [BsonElement("ButikId")]
        [BsonRepresentation(BsonType.ObjectId)] // Refererar till ObjectId i Butiker
        public ObjectId ButikId { get; set; } // Använd ObjectId istället för string

        [BsonElement("ISBN")]
        public string ISBN { get; set; } = null!; // ISBN är en sträng

        [BsonElement("Antal")]
        public int Antal { get; set; } // Antal böcker i lager för den specifika boken
    }
}

