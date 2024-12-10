using MongoDB.Bson;
using MongoDB.Driver;

namespace Laboration_3_databaser
{
    public static class DatabaseManager
    {
        // 🟢 Connection string till MongoDB Atlas
        private static readonly string connectionString = "mongodb+srv://david:123@cluster0.mongodb.net/Bokhandel_2";

        // 🟢 Skapa MongoDB-klienten och databasen
        private static readonly MongoClient client = new MongoClient(connectionString);
        private static readonly IMongoDatabase database = client.GetDatabase("LaborationDatabase");

        // 🟢 Metod för att hämta Butiker-samlingen (starkt typad)
        public static IMongoCollection<Butiker> GetButikerCollection()
        {
            return database.GetCollection<Butiker>("Butiker");
        }

        // 🟢 Metod för att hämta Lagersaldo-samlingen (starkt typad)
        public static IMongoCollection<Lagersaldo> GetLagersaldoCollection()
        {
            return database.GetCollection<Lagersaldo>("Lagersaldo");
        }

        // 🟢 Metod för att hämta Böcker-samlingen (starkt typad)
        public static IMongoCollection<Böcker> GetBooksCollection()
        {
            return database.GetCollection<Böcker>("Böcker");
        }
    }
}


