using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laboration_3_databaser
{
    public class Böcker
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("Titel")]
        public string Titel { get; set; }

        [BsonElement("Författare")]  // Lägg till detta fält om det saknas
        public string Författare { get; set; }  // Författaren kan vara en enkel sträng, eller du kan använda en referens till en annan klass

        [BsonElement("Pris")]
        public decimal Pris { get; set; }

        [BsonElement("Utgivningsdatum")]
        public DateTime Utgivningsdatum { get; set; }
    }
}


