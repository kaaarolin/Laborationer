
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop
{
    class Main_menu
    {
        static List<Customer> customers = new List<Customer>
        {
            new Customer("Knatte", "123"),
            new Customer("Fnatte", "321"),
            new Customer("Tjatte", "231")
        };

        static List<Items> items = new List<Items>
        {
            new Items("Klänning", 3000),
            new Items("Tröja", 4000),
            new Items("Byxor", 5000)
        };

        static public void MainMenu()
        {
            Customer loggedInCustomer = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Karolin's shop, would you like to log in, register, or exit?");
                Console.WriteLine("1. Log in ");
                Console.WriteLine("2. Register as a new customer");
                Console.WriteLine("3. Exit");

                int choice = 0;
                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
                {
                    Console.WriteLine("Invalid input. Please choose a valid option.");
                    continue;
                }

                if (choice == 1)
                {
                    LogInMenu loginMenu = new LogInMenu();
                    loggedInCustomer = loginMenu.LogIn();

                    if (loggedInCustomer != null)
                    {
                        loginMenu.ShoppingMenu(loggedInCustomer);
                    }
                }
                else if (choice == 2)
                {
                    LogInMenu loginMenu = new LogInMenu();
                    loginMenu.Register();
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Thank you for visiting Karolin's shop! Goodbye!");
                    break;
                }
            }
        }

        static void Main(string[] args)
        {
            MainMenu();
        }

        public class LogInMenu
        {
            public Customer LogIn()
            {
                Console.Clear();
                Console.WriteLine("Please enter your username:");
                string username = Console.ReadLine();

                Customer existingCustomer = customers.FirstOrDefault(c => c.Name == username);

                if (existingCustomer == null)
                {
                    Console.WriteLine("Customer not found. Would you like to register a new customer? (yes/no)");
                    string registerResponse = Console.ReadLine().ToLower();

                    if (registerResponse == "yes")
                    {
                        Register();
                    }
                    else
                    {
                        Console.WriteLine("Returning to the main menu.");
                    }
                    return null;
                }

                Console.WriteLine("Please enter your password:");
                string password = Console.ReadLine();

                int attempts = 0;
                while (!existingCustomer.VerifyPassword(password) && attempts < 2)
                {
                    attempts++;
                    Console.WriteLine("Incorrect password. Please try again:");
                    password = Console.ReadLine();
                }

                if (existingCustomer.VerifyPassword(password))
                {
                    Console.WriteLine("Login successful.");
                    return existingCustomer;
                }
                else
                {
                    Console.WriteLine("Too many attempts. Returning to the main menu.");
                    return null;
                }
            }

            public void Register()
            {
                Console.Clear();
                Console.Write("Please enter a username: ");
                string username = Console.ReadLine();

                Console.Write("Please enter a password: ");
                string password = Console.ReadLine();

                customers.Add(new Customer(username, password));
                Console.WriteLine("You are now registered!");
                Console.WriteLine("You can now log in with your new account");
                Console.WriteLine("Press any key to return to the main menu");
                Console.ReadKey();
            }

            public void ShoppingMenu(Customer loggedInCustomer)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Choose one of the following options:");
                    Console.WriteLine("1. Shop");
                    Console.WriteLine("2. View Cart");
                    Console.WriteLine("3. Checkout");
                    Console.WriteLine("4. Log Out");

                    int menuChoice = 0;
                    if (!int.TryParse(Console.ReadLine(), out menuChoice) || menuChoice < 1 || menuChoice > 4)
                    {
                        Console.WriteLine("Invalid input, please choose a valid option.");
                        continue;
                    }

                    if (menuChoice == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Available products:");
                        for (int i = 0; i < Main_menu.items.Count; i++)
                        {
                            var item = Main_menu.items[i];
                            Console.WriteLine($"{i + 1}. {item.Name} - Price: {item.Price} SEK");
                        }

                        Console.WriteLine("Enter the number of the product you want to add to your cart:");
                        int productChoice = 0;
                        if (int.TryParse(Console.ReadLine(), out productChoice) && productChoice > 0 && productChoice <= Main_menu.items.Count)
                        {
                            var selectedItem = Main_menu.items[productChoice - 1];
                            loggedInCustomer.AddToCart(selectedItem);
                        }
                        else
                        {
                            Console.WriteLine("Invalid product choice.");
                        }

                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                    }
                    else if (menuChoice == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("Your cart:");

                        if (loggedInCustomer.Cart.Count == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                        }
                        else
                        {
                            foreach (var item in loggedInCustomer.Cart)
                            {
                                Console.WriteLine($"{item.Name} - Price: {item.Price} SEK, Quantity: {item.Quantity}");
                            }
                            Console.WriteLine($"Total: {loggedInCustomer.CalculateTotalPrice()} SEK");
                        }

                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                    }
                    else if (menuChoice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Checkout");

                        if (loggedInCustomer.Cart.Count == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                        }
                        else
                        {
                            decimal totalPrice = loggedInCustomer.CalculateTotalPrice();
                            Console.WriteLine($"Your total is: {totalPrice} SEK");
                            Console.WriteLine("Do you want to proceed with the checkout? (yes/no)");
                            string confirmCheckout = Console.ReadLine().ToLower();

                            if (confirmCheckout == "yes")
                            {
                                loggedInCustomer.Cart.Clear();
                                Console.WriteLine("Thank you for your purchase!");
                            }
                            else
                            {
                                Console.WriteLine("Checkout cancelled.");
                            }
                        }

                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                    }
                    else if (menuChoice == 4)
                    {
                        Console.WriteLine("You have logged out. Returning to the main menu.");
                        break; 
                    }
                }
            }

        }
    }

}
 

