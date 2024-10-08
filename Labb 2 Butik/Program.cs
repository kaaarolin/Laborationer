
using System.Security.Cryptography.X509Certificates;

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

                new Items("1. Klänning ", 3000),
                new Items("2. Tröja ", 4000),
                new Items("3. Byxor ", 5000)
            };

        static public void MainMenu()
        {
            Console.WriteLine("Welcome to Karolin's shop, would you like to log in or register as a new customer?");
            Console.WriteLine("1. Log in ");
            Console.WriteLine("2. Register as a new customer");

            int choice = 0;
            choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                LogInMenu loginMenu = new LogInMenu();
                loginMenu.LogIn();
            }
            else if (choice == 2)
            {
                LogInMenu logInMenu2 = new LogInMenu();
                logInMenu2.Register();
            }

            else
            {
                Console.Clear();
                Console.WriteLine("We hope to see you here another time!");

            }
            
        }

        static void Main(string[] args)
        {
            MainMenu();
        }

        public class LogInMenu
        {
          public void LogIn()
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
                        Main_menu.MainMenu();
                    }
                    return;
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
                    Console.WriteLine("Login successful");
                    
                    ShoppingMenu(existingCustomer);
                }
                else
                {
                    Console.WriteLine("Too many attempts. Returning to main menu.");
                    Main_menu.MainMenu();
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

                Main_menu.MainMenu();
            }

            public void ShoppingMenu(Customer loggedInCustomer)
            {
                while (true)
                {

                    Console.Clear();
                    Console.WriteLine("Choose one of the following: ");
                    Console.WriteLine("1. Shop");
                    Console.WriteLine("2. View Cart");
                    Console.WriteLine("3. Checkout");
                    Console.WriteLine("4. Exit");

                    int Menuchoice = 0;
                    if (!int.TryParse(Console.ReadLine(), out Menuchoice) || Menuchoice < 1 || Menuchoice > 4)
                    {
                        Console.WriteLine("Invalid input, please choose a valid option.");
                        continue;
                    }

                    if (Menuchoice == 1)
                    {
                        bool KeepShopping = true;

                        while (KeepShopping)
                        {
                            foreach (var i in items)
                            {
                                
                                Console.WriteLine(i.ToString());
                            }

                            int productChoice = 0;
                            Console.WriteLine("Please choose a product to add to your cart (1 - 3) Price is shown in sek: ");
                            productChoice = int.Parse(Console.ReadLine());

                            if (productChoice >= 0 && productChoice <= items.Count)
                            {
                                loggedInCustomer.AddToCart(items[productChoice - 1]);
                            }

                            else

                            {
                                Console.WriteLine("Error. Please try again.");
                                continue;

                            }

                            Console.WriteLine("Would you like to continue shopping? (yes/no)");
                            string continueShopping = Console.ReadLine().ToLower();

                            if (continueShopping != "yes")
                            {
                                KeepShopping = false;
                            }

                        }
                    }

                    else if (Menuchoice == 2)
                    {
                        Console.Clear();
                        Console.WriteLine("In your cart: ");

                        if (loggedInCustomer.Cart.Count == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                        }
                        else
                        {
                            foreach (var item in loggedInCustomer.Cart)
                            {
                                Console.WriteLine($"{item.Name}, Price: {item.Price} SEK, Quantity: {item.Quantity}");
                            }
                            Console.WriteLine($"Total cart value: {loggedInCustomer.CalculateTotalPrice().ToString("C")}");
                        }

                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                    }


                    else if (Menuchoice == 3)
                    {
                        Console.Clear();
                        Console.WriteLine("Check out");

                        if (loggedInCustomer.Cart.Count == 0)
                        {
                            Console.WriteLine("Your cart is empty.");
                        }
                        else
                        {
                            
                            decimal totalPrice = loggedInCustomer.CalculateTotalPrice();
                            Console.WriteLine("Your total is: " + totalPrice.ToString("C"));

                            
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

                    else if (Menuchoice == 4)
                    {
                        Console.WriteLine("Thank you and please come again another time!");
                        break;
                    }
                }

  
            }


        }
    }
}
