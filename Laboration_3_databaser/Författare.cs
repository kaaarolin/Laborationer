using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laboration_3_databaser
{
    public class Författare
    {
        [BsonId] // Markerar detta fält som primärnyckel
        [BsonRepresentation(BsonType.ObjectId)] // Använder ObjectId som MongoDB:s standard-id
        public ObjectId Id { get; set; } // För att använda MongoDB ObjectId direkt

        [BsonElement("Förnamn")] // Sätter fältnamnet i MongoDB
        public string Förnamn { get; set; } = null!;

        [BsonElement("Efternamn")]
        public string Efternamn { get; set; } = null!;

        [BsonElement("Födelsedatum")]
        public DateTime? Födelsedatum { get; set; } // Bytt från DateOnly till DateTime för MongoDB-kompatibilitet

        [BsonElement("Böcker")]
        public virtual ICollection<string> Böcker { get; set; } = new List<string>(); // Lagrar ISBN för böcker skrivna av författaren
    }
}

