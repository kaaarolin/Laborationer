using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2._databaser
{
    public static class Metoder
    {
        public static void ListInventoryForStore()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    var inventory = context.Lagersaldo
                        .Include(ls => ls.Böcker)
                        .Include(ls => ls.Butiker)
                        .GroupBy(ls => ls.Butiker.Butiksnamn)
                        .Select(group => new
                        {
                            StoreName = group.Key,
                            Books = group.Select(g => new
                            {
                                Isbn = g.Böcker.ISBN13,
                                Title = g.Böcker.Titel,
                                Stock = g.Antal
                            }).ToList()
                        })
                        .OrderBy(store => store.StoreName)
                        .ToList();

                    if (inventory.Count == 0)
                    {
                        Console.WriteLine("Inga böcker finns i lagersaldo.");
                    }
                    else
                    {
                        Console.WriteLine("Lagersaldo per bokhandel:");
                        foreach (var store in inventory)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Bokhandel: {store.StoreName}");
                            Console.ResetColor();

                            foreach (var book in store.Books)
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

                                Console.ResetColor();
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
        }


        public static void AddBookToStoreInventory()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tillgängliga butiker:");
                    Console.ResetColor();
                    var stores = context.Butiker.ToList();
                    foreach (var store in stores)
                    {
                        Console.WriteLine($"ID: {store.ButikId}, Namn: {store.Butiksnamn}");
                    }

                    Console.Write("Ange ID för butiken du vill lägga till böcker i: ");
                    if (!int.TryParse(Console.ReadLine(), out int storeId) || !stores.Any(s => s.ButikId == storeId))
                    {
                        Console.WriteLine("Ogiltigt butik ID. Försök igen.");
                        return;
                    }

                    Console.WriteLine("\nTillgängliga böcker:");
                    var books = context.Böcker.ToList();
                    foreach (var book in books)
                    {
                        Console.WriteLine($"Titel: {book.Titel}, ISBN: {book.ISBN13}");
                    }

                    Console.Write("Ange titeln för boken du vill lägga till: ");
                    string title = Console.ReadLine();
                    var selectedBook = books.FirstOrDefault(b => b.Titel.Equals(title, StringComparison.OrdinalIgnoreCase));
                    if (selectedBook == null)
                    {
                        Console.WriteLine("Ogiltig titel. Försök igen.");
                        return;
                    }

                    Console.Write("Ange antal exemplar: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Ogiltigt antal. Försök igen.");
                        return;
                    }

                    var existingInventory = context.Lagersaldo.FirstOrDefault(ls => ls.ButikId == storeId && ls.Isbn == selectedBook.ISBN13);
                    if (existingInventory != null)
                    {
                        existingInventory.Antal += quantity;
                        Console.WriteLine($"Antalet uppdaterades. Totalt antal: {existingInventory.Antal}");
                    }
                    else
                    {
                        var newInventory = new Lagersaldo
                        {
                            ButikId = storeId,
                            Isbn = selectedBook.ISBN13,
                            Antal = quantity
                        };
                        context.Lagersaldo.Add(newInventory);
                        Console.WriteLine("Bok lades till i lagret.");
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }

        }


        public static void RemoveBookFromInventory()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tillgängliga butiker:");
                    Console.ResetColor();
                    var stores = context.Butiker.ToList();
                    foreach (var store in stores)
                    {
                        Console.WriteLine($"ID: {store.ButikId}, Namn: {store.Butiksnamn}");
                    }

                    Console.Write("Ange ID för butiken du vill ta bort böcker från: ");
                    if (!int.TryParse(Console.ReadLine(), out int storeId) || !stores.Any(s => s.ButikId == storeId))
                    {
                        Console.WriteLine("Ogiltigt butik ID. Försök igen.");
                        return;
                    }

                    var inventory = context.Lagersaldo
                        .Include(ls => ls.Böcker)
                        .Where(ls => ls.ButikId == storeId)
                        .GroupBy(ls => ls.Böcker.ISBN13)
                        .Select(group => new
                        {
                            Isbn = group.Key,
                            Title = group.First().Böcker.Titel,
                            TotalStock = group.Sum(ls => ls.Antal)
                        })
                        .ToList();

                    if (!inventory.Any())
                    {
                        Console.WriteLine("Den valda butiken har inga böcker i lager.");
                        return;
                    }

                    Console.WriteLine("\nLagersaldo för den valda butiken:");
                    foreach (var item in inventory)
                    {
                        Console.WriteLine($"Titel: {item.Title}, ISBN: {item.Isbn}, Antal: {item.TotalStock}");
                    }

                    Console.Write("Ange titel för boken du vill ta bort: ");
                    string title = Console.ReadLine()?.Trim();
                    var selectedBook = inventory.FirstOrDefault(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                    if (selectedBook == null)
                    {
                        Console.WriteLine("Ogiltig titel eller boken finns inte i lagret. Försök igen.");
                        return;
                    }

                    Console.Write("Ange antal exemplar att ta bort: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantityToRemove) || quantityToRemove <= 0)
                    {
                        Console.WriteLine("Ogiltigt antal. Försök igen.");
                        return;
                    }

                    var lagersaldoToUpdate = context.Lagersaldo
                        .FirstOrDefault(ls => ls.ButikId == storeId && ls.Böcker.ISBN13 == selectedBook.Isbn);

                    if (lagersaldoToUpdate == null)
                    {
                        Console.WriteLine("Boken kunde inte hittas i lagret. Försök igen.");
                        return;
                    }

                    if (quantityToRemove >= lagersaldoToUpdate.Antal)
                    {
                        context.Lagersaldo.Remove(lagersaldoToUpdate);
                        Console.WriteLine("Boken togs bort helt från lagret.");
                    }
                    else
                    {
                        lagersaldoToUpdate.Antal -= quantityToRemove;
                        Console.WriteLine($"Antalet uppdaterades. Totalt kvar: {lagersaldoToUpdate.Antal}");
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }


        public static void ListAllBooks()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    var books = context.Böcker
                        .Include(b => b.Författare)
                        .ToList();

                    if (!books.Any())
                    {
                        Console.WriteLine("Inga böcker finns i sortimentet.");
                        return;
                    }

                    Console.WriteLine("Böcker i sortimentet:");
                    foreach (var book in books)
                    {
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.Write($"Titel: ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"{book.Titel}");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"  ISBN: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"{book.ISBN13}");

                        if (book.Författare != null)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"  Författare: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{book.Författare.Förnamn}");
                        }

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"  Pris: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{book.Pris:C}");

                        if (book.Utgivningsdatum.HasValue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"  Utgivningsdatum: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{book.Utgivningsdatum:yyyy-MM-dd}");
                        }

                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }


        public static void AddNewBook()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    Console.WriteLine("Tillgängliga författare:");
                    var authors = context.Författare.ToList();
                    foreach (var author in authors)
                    {
                        Console.WriteLine($"ID: {author.Id}, Namn: {author.Förnamn}");
                    }

                    Console.Write("Vill du använda en befintlig författare? (ja/nej): ");
                    string useExistingAuthor = Console.ReadLine()?.Trim().ToLower();

                    int authorId;

                    if (useExistingAuthor == "ja")
                    {
                        Console.Write("Ange författarens ID: ");
                        if (!int.TryParse(Console.ReadLine(), out authorId) || !authors.Any(a => a.Id == authorId))
                        {
                            Console.WriteLine("Ogiltigt ID. Försök igen.");
                            return;
                        }
                    }
                    else
                    {
                        authorId = AddNewAuthor(context);
                    }

                    Console.Write("Ange titel: ");
                    string title = Console.ReadLine();

                    Console.Write("Ange ISBN (13 siffror): ");
                    string isbn = Console.ReadLine();

                    if (isbn.Length != 13)
                    {
                        Console.WriteLine("ISBN måste vara exakt 13 tecken.");
                        return;
                    }

                    Console.Write("Ange antal sidor: ");
                    if (!int.TryParse(Console.ReadLine(), out int pages) || pages <= 0)
                    {
                        Console.WriteLine("Antal sidor måste vara ett positivt tal.");
                        return;
                    }

                    Console.Write("Ange pris: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
                    {
                        Console.WriteLine("Pris måste vara ett positivt tal.");
                        return;
                    }

                    Console.Write("Ange utgivningsdatum (YYYY-MM-DD): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime publicationDate))
                    {
                        Console.WriteLine("Ogiltigt datumformat.");
                        return;
                    }

                    Console.Write("Ange språk: ");
                    string language = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(language))
                    {
                        Console.WriteLine("Språk är obligatoriskt.");
                        return;
                    }

                    var newBook = new Böcker
                    {
                        Titel = title,
                        ISBN13 = isbn,
                        AntalSidor = pages,
                        Pris = price,
                        Utgivningsdatum = DateOnly.FromDateTime(publicationDate),
                        FörfattareId = authorId,
                        Språk = language
                    };

                    context.Böcker.Add(newBook);
                    context.SaveChanges();

                    Console.WriteLine("Ny bok lades till i sortimentet.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }

        public static int AddNewAuthor(BokhandelContext context)
        {
            Console.Clear();

            try
            {
                Console.WriteLine("Lägg till en ny författare");

                // Ta in förnamn
                Console.Write("Ange författarens förnamn: ");
                string firstName = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("Förnamn är obligatoriskt.");
                    return -1;
                }

                // Ta in efternamn
                Console.Write("Ange författarens efternamn: ");
                string lastName = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Efternamn är obligatoriskt.");
                    return -1;
                }

                // Ta in födelsedatum
                Console.Write("Ange författarens födelsedatum (YYYY-MM-DD): ");
                DateOnly? birthDate = null;
                string birthDateInput = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(birthDateInput))
                {
                    if (!DateOnly.TryParse(birthDateInput, out DateOnly parsedDate))
                    {
                        Console.WriteLine("Ogiltigt datumformat.");
                        return -1;
                    }
                    birthDate = parsedDate;
                }

                // Skapa en ny författare
                var newAuthor = new Författare
                {
                    Förnamn = firstName,
                    Efternamn = lastName,
                    Födelsedatum = birthDate // Kan vara null om användaren inte anger ett datum
                };

                // Lägg till författaren i databasen
                context.Författare.Add(newAuthor);
                context.SaveChanges();

                Console.WriteLine($"Författaren {firstName} {lastName} lades till med ID {newAuthor.Id}.");
                return newAuthor.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return -1;
            }
            finally
            {
                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }



        public static void DeleteBook()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    Console.Write("Ange titeln för boken du vill ta bort: ");
                    string title = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine("Titeln får inte vara tom.");
                        return;
                    }

                    // Hämta boken med titeln (skiftlägesokänslig)
                    var book = context.Böcker
                        .FirstOrDefault(b => b.Titel.ToLower() == title.ToLower());

                    if (book == null)
                    {
                        Console.WriteLine("Boken med den angivna titeln hittades inte.");
                        return;
                    }

                    context.Böcker.Remove(book);
                    context.SaveChanges();

                    Console.WriteLine($"Boken '{book.Titel}' togs bort.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }


        public static void DeleteAuthor()
        {
            using (var context = new BokhandelContext())
            {
                Console.Clear();

                try
                {
                    // Lista alla författare
                    Console.WriteLine("Tillgängliga författare:");
                    var authors = context.Författare.ToList();

                    if (!authors.Any())
                    {
                        Console.WriteLine("Inga författare finns i databasen.");
                        return;
                    }

                    foreach (var authorListItem in authors)
                    {
                        Console.WriteLine($"ID: {authorListItem.Id}, Namn: {authorListItem.Förnamn} {authorListItem.Efternamn}");
                    }

                    Console.Write("\nAnge ID för författaren du vill ta bort: ");
                    if (!int.TryParse(Console.ReadLine(), out int authorId))
                    {
                        Console.WriteLine("Ogiltigt ID. Försök igen.");
                        return;
                    }

                    // Hämta författaren och dess kopplade böcker
                    var author = context.Författare
                        .Include(a => a.Böcker) // Inkludera kopplade böcker
                        .FirstOrDefault(a => a.Id == authorId);

                    if (author == null)
                    {
                        Console.WriteLine("Författaren hittades inte.");
                        return;
                    }

                    if (author.Böcker.Any())
                    {
                        Console.WriteLine("Författaren kan inte tas bort eftersom det finns böcker kopplade till denne.");
                        return;
                    }

                    // Ta bort författaren
                    context.Författare.Remove(author);
                    context.SaveChanges();

                    Console.WriteLine($"Författaren {author.Förnamn} {author.Efternamn} togs bort.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }

                Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }

    }
}

// ok 