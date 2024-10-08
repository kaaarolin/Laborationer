using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Shop
{
    public class Customer
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        private List<Items> _cart;
        public List<Items> Cart { get { return _cart; } }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<Items>();

           
        }

        public void AddToCart(Items item)
        {
            var existingItem = Cart.Find(i => i.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Cart.Add(item);
            }

            Console.WriteLine(item.Name + " was added to your cart");
        }

        public override string ToString()
        {
            if (Cart.Count == 0)
            {
                return Name + " your cart is empty";
            }

            string cartSummary = $"Customer: {Name}\nCart items: {Cart.Count}\n";

            foreach (var item in Cart)
            {
                cartSummary += $"{item.Name} - unit price: {item.Price:C}, Quantity: {item.Quantity}, Total: {item.TotalPrice():C}\n";
            }

            cartSummary += $"Total cost of cart: {CalculateTotalPrice():C}";
            return cartSummary;
        }


        public bool VerifyPassword(string password)
        {
            for (int attempt = 0; attempt <= 3; attempt++)
            {

                if (password == Password)
                {
                    return true;
                }
               
            }
            
            return false;

        }

        public decimal CalculateTotalPrice()
        {
            decimal total = 0;

            foreach (var item in Cart)
            {
                
                total += item.Price * item.Quantity; 
            }

            return total;
        }
    
    }
}


