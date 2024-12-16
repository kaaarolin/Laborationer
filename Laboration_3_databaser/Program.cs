using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Laboration_3_databaser;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laboration_3_databaser
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
                        Metoder.ListInventory();
                        break;
                    case "2":
                        Metoder.AddProductToStoreInventory();
                        break;
                    case "3":
                        Metoder.RemoveProductFromInventory();
                        break;
                    case "4":
                        Metoder.ViewCart();
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
            Console.WriteLine("Välkommen till Karolins butik! Välj ett alternativ: ");
            Console.ResetColor();
            Console.WriteLine("1. Lista sortiment");
            Console.WriteLine("2. Lägg till produkter i sortiment");
            Console.WriteLine("3. Ta bort produkter från sortiment");
            Console.WriteLine("4. Se kundvagn");
            Console.WriteLine("0. Avsluta");
        }

    }

}