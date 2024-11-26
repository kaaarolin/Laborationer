using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_2_databaser;
public class Program
{
    public static void Main(string[] args)
    {
        var Metoder = new Metoder();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Karolins bokhandel. Var god välj ett av förljande: ");
            Console.WriteLine("1. Lista lagersaldo för varje bokhandel");
            Console.WriteLine("2. Lägg till bok i lager");
            Console.WriteLine("3. Ta bort bok från lager");
            Console.WriteLine("4. Avsluta");

            int choice = 0;

            if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Ogiltigt val. Var god testa igen.");
                continue;
            }

            if (choice == 1)
            {
                Metoder.ListInventoryBookstore();
            }

            else if (choice == 2)
            {
                // AddBooktoInventory();
            }

            else if (choice == 3)
            {
                // RemoveBookfromInventory();
            }

            else if (choice == 4)
            {
                Console.WriteLine("Tack för ditt besök!");
                break;
            }
        }
    }
}