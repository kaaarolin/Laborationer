using Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop

{
    public class Items
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public int Quantity { get; set; }
        public Items(string name, decimal price)
        {
            Name = name;
            Price = price;
            Quantity = 1;
        }

        

        public decimal TotalPrice()
        {
            return Price * Quantity;
        }
        

        public override string ToString()
        {
            return Name + "- Price: " + Price + ", Quantity: " + Quantity + ", Total: " + TotalPrice();

        }
    }

}
