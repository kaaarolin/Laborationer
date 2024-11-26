using Microsoft.EntityFrameworkCore;

namespace Labb_2_databaser.Models2;

public class Metoder
{
    private readonly BokhandelContext _context;

    public Metoder()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BokhandelContext>();
        optionsBuilder.UseSqlServer(@"Data Source=localhost;Database=Laboration_1;Integrated Security=True;TrustServerCertificate=True;");
        _context = new BokhandelContext(optionsBuilder.Options);
    }

    public void ListInventory()
    {
        var stores = _context.Butikerna
            .Include(b => b.Lagersaldo)
            .ThenInclude(ls => ls.Böcker)
            .ToList();

        Console.Clear();
        Console.WriteLine("Lagersaldo för alla butiker:");
        foreach (var store in stores)
        {
            Console.WriteLine($"\nButik: {store.Butiksnamn} - Adress: {store.Adress}");
            if (store.Lagersaldo.Any())
            {
                foreach (var item in store.Lagersaldo)
                {
                    Console.WriteLine($"  Bok: {item.Böcker.Titel}, Antal: {item.Antal}");
                }
            }
            else
            {
                Console.WriteLine("  Inga böcker i lager.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Laboration_2._databaser;
using Microsoft.EntityFrameworkCore;

namespace Laboration_2._databaser
{
    public class Program
    {
        // Din Main-metod
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Välkommen till Karolins sida för varustyrning! Välj ett alternativ:");
                Console.ResetColor();
                Console.WriteLine("1. Lista lagersaldo");
                Console.WriteLine("2. Lägg till bok i lager");
                Console.WriteLine("3. Ta bort bok från lager");
                Console.WriteLine("4. Avsluta");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ListInventory();
                            break;
                        case 2:
                            AddBookToStore();
                            break;
                        case 3:
                            RemoveBookFromStore();
                            break;
                        case 4:
                            Console.WriteLine("Tack för ditt besök! Välkommen åter!");
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val. Försök igen.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Ange ett nummer.");
                }

                Console.WriteLine("Tryck på valfri tangent för att komma tillbaka till huvudmenyn");
                Console.ReadKey();
            }
        }

        // Metod för att lista lagersaldo
        public static void ListInventory()
        {
            using (var context = new BokhandelContext())
            {
                var stores = context.Butiker
                    .Include(b => b.Lagersaldo)
                    .ThenInclude(ls => ls.Böcker)
                    .AsNoTracking()
                    .ToList();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Lagersaldo för alla butiker:");
                Console.ResetColor();
                foreach (var store in stores)
                {
                    Console.WriteLine($"Butik: {store.Butiksnamn}");
                    foreach (var item in store.Lagersaldo)
                    {
                        Console.WriteLine($"  Bok: {item.Böcker.Titel}, Antal: {item.Antal}");
                    }
                }
            }
        }

        // Metod för att lägga till böcker i lager
        public static void AddBookToStore()
        {
            using (var context = new BokhandelContext())
            {
                Console.Write("Ange butikens ID: ");
                int butikId = int.Parse(Console.ReadLine());

                Console.WriteLine("Tillgängliga böcker i sortimentet:");
                var allBooks = context.Böcker.ToList();
                foreach (var book in allBooks)
                {
                    Console.WriteLine($"ISBN: {book.ISBN13}, Titel: {book.Titel}");
                }

                Console.Write("Ange ISBN för boken du vill lägga till: ");
                string isbn = Console.ReadLine();

                Console.Write("Ange antal: ");
                int antal = int.Parse(Console.ReadLine());

                var newInventory = new Lagersaldo
                {
                    ButikId = butikId,
                    Isbn = isbn,
                    Antal = antal
                };

                context.Lagersaldo.Add(newInventory);
                context.SaveChanges();

                Console.WriteLine("Bok lades till i lagret.");
            }
        }

        // Metod för att ta bort böcker från lager
        public static void RemoveBookFromStore()
        {
            using (var context = new BokhandelContext())
            {
                Console.Write("Ange butikens ID: ");
                int butikId = int.Parse(Console.ReadLine());

                Console.Write("Ange ISBN för boken du vill ta bort: ");
                string isbn = Console.ReadLine();

                var inventory = context.Lagersaldo
                    .FirstOrDefault(ls => ls.ButikId == butikId && ls.Isbn == isbn);

                if (inventory != null)
                {
                    context.Lagersaldo.Remove(inventory);
                    context.SaveChanges();
                    Console.WriteLine("Bok togs bort från lagret.");
                }
                else
                {
                    Console.WriteLine("Boken hittades inte i lagret.");
                }
            }
        }
    }

}


