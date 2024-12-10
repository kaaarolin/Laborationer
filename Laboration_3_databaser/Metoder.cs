using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Laboration_3_databaser
{
    public static class Metoder
    {
        // Samlingar för Butiker, Lagersaldo och Böcker
        private static readonly IMongoCollection<Butiker> butikerCollection = DatabaseManager.GetButikerCollection();  // Stark typ
        private static readonly IMongoCollection<Lagersaldo> lagersaldoCollection = DatabaseManager.GetLagersaldoCollection(); // Stark typ
        private static readonly IMongoCollection<Böcker> booksCollection = DatabaseManager.GetBooksCollection(); // Stark typ

        public static void ListInventoryForStore()
        {
            try
            {
                // Använd starkt typad samling här
                var inventory = lagersaldoCollection.Aggregate()
                    .Lookup(
                        "Böcker",        // Samling: Böcker
                        "ISBN",          // Fält i Lagersaldo
                        "_id",           // Matchande fält i Böcker
                        "BöckerInfo"     // Alias för resultat
                    )
                    .Lookup(
                        "Butiker",       // Samling: Butiker
                        "ButikId",       // Fält i Lagersaldo
                        "_id",           // Matchande fält i Butiker
                        "ButikerInfo"    // Alias för resultat
                    )
                    .As<BsonDocument>()
                    .ToList();

                // Logga resultat efter Lookup för felsökning
                Console.WriteLine("Efter Lookup:");
                foreach (var doc in inventory)
                {
                    Console.WriteLine(doc.ToJson());
                }

                // Filtrera och hantera dokument
                var filteredInventory = inventory
                    .Where(doc =>
                        doc.Contains("ButikerInfo") && doc["ButikerInfo"].AsBsonArray.Count > 0 &&
                        doc.Contains("BöckerInfo") && doc["BöckerInfo"].AsBsonArray.Count > 0)
                    .GroupBy(doc => doc["ButikerInfo"][0]["Butiksnamn"].AsString)
                    .Select(group => new
                    {
                        StoreName = group.Key,
                        Books = group.Select(g => new
                        {
                            Isbn = g["BöckerInfo"][0]["_id"].AsString,  // Hämtar _id som ISBN
                            Title = g["BöckerInfo"][0]["Titel"].AsString,
                            Stock = g["Antal"].AsInt32
                        }).ToList()
                    })
                    .OrderBy(store => store.StoreName)
                    .ToList();

                // Kontrollera om det finns lagersaldo
                if (filteredInventory.Count == 0)
                {
                    Console.WriteLine("Inga böcker finns i lagersaldo.");
                }
                else
                {
                    Console.WriteLine("Lagersaldo per bokhandel:");
                    foreach (var store in filteredInventory)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Bokhandel: {store.StoreName}");
                        Console.ResetColor();

                        foreach (var book in store.Books)
                        {
                            // Kontrollera om ISBN, titel och antal är giltiga
                            if (!string.IsNullOrEmpty(book.Isbn) && !string.IsNullOrEmpty(book.Title) && book.Stock > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"  ISBN: ");
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write($"{book.Isbn}, ");

                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"Titel: ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write($"{book.Title}");

                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($", Antal: ");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"{book.Stock}");
                            }
                        }

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid hämtning av lagersaldo: {ex.Message}");
            }

            Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
            Console.ReadKey();
        }


        public static async Task AddBookToStoreInventory()
        {
            try
            {
                Console.WriteLine("📘 Ange titel på boken:");
                string title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("❌ Titel får inte vara tom.");
                    return;
                }

                Console.WriteLine("👤 Ange författare:");
                string author = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(author))
                {
                    Console.WriteLine("❌ Författare får inte vara tom.");
                    return;
                }

                Console.WriteLine("💰 Ange pris:");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                {
                    Console.WriteLine("❌ Ogiltigt pris. Försök igen.");
                    return;
                }

                // Skapa en instans av den starkt typade klassen
                var newBook = new Böcker
                {
                    ISBN = "9781234567891",  // Exempel på ISBN
                    Titel = title,
                    Författare = author,
                    Pris = price,
                    Utgivningsdatum = DateTime.Now  // Använd det aktuella datumet som exempel
                };

                // Infoga den starkt typade boken i samlingen
                await booksCollection.InsertOneAsync(newBook);
                Console.WriteLine("✅ Bok tillagd i lagret.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Ett fel uppstod: {ex.Message}");
            }
        }


        public static async Task RemoveBookFromInventory()
        {
            try
            {
                Console.WriteLine("🗑️ Ange titeln på boken du vill ta bort:");
                string title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("❌ Titel får inte vara tom.");
                    return;
                }

                // Skapa ett filter för att matcha boken baserat på titel
                var filter = Builders<Böcker>.Filter.Eq(b => b.Titel, title);

                // Använd rätt samlingstyp och rätt filter
                var result = await booksCollection.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine("✅ Boken togs bort från lagret.");
                }
                else
                {
                    Console.WriteLine("❌ Ingen bok med den titeln hittades.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Ett fel uppstod: {ex.Message}");
            }
        }

        public static async Task ListAllBooks()
        {
            try
            {
                Console.WriteLine("📚 Alla böcker i databasen:");
                var books = await booksCollection.Find(new BsonDocument()).ToListAsync();
                foreach (var book in books)
                {
                    Console.WriteLine(book.ToJson());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Ett fel uppstod: {ex.Message}");
            }
        }
    }
}

