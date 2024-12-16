using Laboration_3_databaser;
using MongoDB.Driver;

namespace Laboration_3_databaser
{
    public class DatabaseManager
    {
        private readonly IMongoDatabase _database;

        public DatabaseManager()
        {
            // Använd din egen anslutningssträng från MongoDB Atlas
            var client = new MongoClient("mongodb+srv://david:123@school.37vmr.mongodb.net/");
            _database = client.GetDatabase("Butik");
        }

        public IMongoCollection<Sortiment> GetSortimentCollection()
        {
            return _database.GetCollection<Sortiment>("Sortiment");
        }
        
    }
}

