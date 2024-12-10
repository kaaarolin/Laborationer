using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laboration_3_databaser
{
    public class Butiker
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); // Unik identifierare för MongoDB

        [BsonElement("Butiksnamn")]
        public string Butiksnamn { get; set; } = null!;

        [BsonElement("Adress")]
        public string Adress { get; set; } = null!;

        [BsonElement("Stad")]
        public string Stad { get; set; } = null!;

        [BsonElement("Postnummer")]
        public string? Postnummer { get; set; }

        [BsonElement("Land")]
        public string Land { get; set; } = null!;

        [BsonElement("Telefon")]
        public string? Telefon { get; set; }

        [BsonElement("Lagersaldo")]
        public List<Lagersaldo> Lagersaldo { get; set; } = new List<Lagersaldo>(); // Inbäddade lagersaldodata
    }
}
