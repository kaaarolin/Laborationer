using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Laboration_3_databaser
{
    public static class Metoder
    {
        // Anslut till MongoDB och hämta kollektionen 'sortiment'

        private static List<BsonDocument> kundvagn = new List<BsonDocument>();
        private static IMongoCollection<BsonDocument> GetSortimentCollection()
        {
            try
            {
                var client = new MongoClient("mongodb+srv://david:123@school.37vmr.mongodb.net/");
                var database = client.GetDatabase("Butik");
                var collection = database.GetCollection<BsonDocument>("Sortiment");
                return collection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid anslutning till databasen: {ex.Message}");
                throw;
            }
        }

        // Metod för att visa alla produkter i sortimentet
        public static void ListInventory()
        {
            try
            {
                var sortimentCollection = GetSortimentCollection();

                // Hämta och sortera alla produkter från kollektionen (sorterar efter namn)
                var allProducts = sortimentCollection.Find(new BsonDocument())
                                                     .SortBy(product => product["name"])
                                                     .ToList();

                Console.WriteLine("\nProdukter i sortimentet:");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("{0,-5} {1,-25} {2,10} {3,24}", "Nr", "Namn", "Pris (SEK)", "ID");
                Console.WriteLine("------------------------------------------------------------");

                if (allProducts.Count == 0)
                {
                    Console.WriteLine("Inga produkter hittades i sortimentet.");
                }
                else
                {
                    for (int i = 0; i < allProducts.Count; i++)
                    {
                        var product = allProducts[i];
                        string name = product.Contains("name") ? product["name"].ToString() : "Okänd";
                        string price = product.Contains("price") ? product["price"].ToString() : "0";
                        string id = product.Contains("_id") ? product["_id"].ToString() : "Okänt ID";

                        // Utskrift med tabellstruktur (kolumner för Nr, namn, pris och ID)
                        Console.WriteLine("{0,-5} {1,-25} {2,10} {3,24}", i + 1, name, price + " SEK", id);
                    }
                }

                Console.WriteLine("------------------------------------------------------------");

                // Be användaren välja en produkt att lägga i kundvagnen
                Console.Write("Ange produktens nummer för att lägga till i kundvagnen (eller tryck ENTER för att avsluta): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Avslutar utan att lägga till i kundvagnen.");
                    return;
                }

                if (int.TryParse(input, out int productNumber) && productNumber > 0 && productNumber <= allProducts.Count)
                {
                    var selectedProduct = allProducts[productNumber - 1];
                    kundvagn.Add(selectedProduct);
                    Console.WriteLine($"Produkten \"{selectedProduct["name"]}\" har lagts till i din kundvagn.");
                }
                else
                {
                    Console.WriteLine("Felaktig inmatning. Vänligen ange ett giltigt produktnummer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid hämtning av sortimentet: {ex.Message}");
            }
        }

        public static void AddProductToStoreInventory()
        {
            try
            {
                var sortimentCollection = GetSortimentCollection();

                // Be användaren att ange produktinformation
                Console.Write("Ange produktens namn: ");
                string productName = Console.ReadLine();

                // Kontroll för tom inmatning
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Produkten måste ha ett namn. Försök igen.");
                    return;
                }

                Console.Write("Ange produktens pris (SEK): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal productPrice) || productPrice <= 0)
                {
                    Console.WriteLine("Felaktig prisinmatning. Priset måste vara ett positivt tal.");
                    return;
                }

                // Skapa ett nytt dokument för produkten
                var newProduct = new BsonDocument
                {
                    { "name", productName },
                    { "price", productPrice }
                };

                // Lägg till produkten i sortimentet
                sortimentCollection.InsertOne(newProduct);

                Console.WriteLine("Produkten har lagts till i sortimentet:");
                Console.WriteLine($"Namn: {productName}, Pris: {productPrice} SEK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid tillägg av produkten: {ex.Message}");
            }
        }

        public static void RemoveProductFromInventory()
        {
            try
            {
                var sortimentCollection = GetSortimentCollection();

                // Be användaren att ange produktens namn
                Console.Write("Ange produktens namn som du vill ta bort: ");
                string productName = Console.ReadLine();

                // Kontroll för tom inmatning
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Produkten måste ha ett namn. Försök igen.");
                    return;
                }

                // Skapa ett filter för att hitta produkten med det givna namnet
                var filter = Builders<BsonDocument>.Filter.Eq("name", productName);

                // Försök att ta bort produkten
                var result = sortimentCollection.DeleteOne(filter);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine($"Produkten \"{productName}\" har tagits bort från sortimentet.");
                }
                else
                {
                    Console.WriteLine($"Produkten med namnet \"{productName}\" kunde inte hittas i sortimentet.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel uppstod vid borttagning av produkten: {ex.Message}");
            }
        }

        public static void ViewCart()
        {
            Console.WriteLine("\nProdukter i din kundvagn:");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("{0,-25} {1,10} {2,24}", "Namn", "Pris (SEK)", "ID");
            Console.WriteLine("------------------------------------------------------------");

            if (kundvagn.Count == 0)
            {
                Console.WriteLine("Din kundvagn är tom.");
            }
            else
            {
                foreach (var product in kundvagn)
                {
                    string name = product.Contains("name") ? product["name"].ToString() : "Okänd";
                    string price = product.Contains("price") ? product["price"].ToString() : "0";
                    string id = product.Contains("_id") ? product["_id"].ToString() : "Okänt ID";

                    // Utskrift med tabellstruktur (kolumner för namn, pris och ID)
                    Console.WriteLine("{0,-25} {1,10} {2,24}", name, price + " SEK", id);
                }
            }

            Console.WriteLine("------------------------------------------------------------");
        }
    }
}


    


