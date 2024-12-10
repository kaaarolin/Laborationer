using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laboration_3_databaser
{
    class Program
    {
        // Länken till din MongoDB Atlas (Uppdatera med ditt användarnamn, lösenord och databas)
        private static readonly string connectionString = "mongodb+srv://<username>:<password>@cluster0.mongodb.net/?retryWrites=true&w=majority";
        private static readonly MongoClient client = new MongoClient(connectionString);
        private static readonly IMongoDatabase database = client.GetDatabase("LaborationDatabase");
        private static readonly IMongoCollection<BsonDocument> booksCollection = database.GetCollection<BsonDocument>("Books");

        public static void Main(string[] args)
        {
            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Metoder.ListInventoryForStore();
                        break;
                    case "2":
                        Metoder.AddBookToStoreInventory();
                        break;
                    case "3":
                        Metoder.RemoveBookFromInventory();
                        break;
                    case "4":
                        Metoder.ListAllBooks();
                        break;
                    case "5":
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("1. Lista lager för butiken");
            Console.WriteLine("2. Lägg till en bok i lagret");
            Console.WriteLine("3. Ta bort en bok från lagret");
            Console.WriteLine("4. Lista alla böcker");
            Console.WriteLine("5. Avsluta");
        }
    }
}
