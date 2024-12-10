using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Laboration_2._databaser
{
    class Program
    {
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
                        Metoder.AddNewBook();
                        break;
                    case "6":
                        Metoder.DeleteBook();
                        break;
                    case "7":
                        using (var context = new BokhandelContext())
                        {
                            Metoder.AddNewAuthor(context);
                        }
                        break;
                    case "8":
                        Metoder.DeleteAuthor();
                        break;

                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

                Console.WriteLine("Tryck på valfri tangent för att komma tillbaka till huvudmenyn...");
                Console.ReadKey();
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Välkommen till Karolins varustyrning! Välj ett alternativ: ");
            Console.ResetColor();
            Console.WriteLine("1. Lista lagersaldo för varje butik");
            Console.WriteLine("2. Lägg till böcker i lager");
            Console.WriteLine("3. Ta bort böcker från lager");
            Console.WriteLine("4. Lista böcker");
        }

    }

}
