using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        public Product()
        {
            Sales = new List<Sale>();
        }
        public ICollection<Sale> Sales { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public string Desciption { get; set; }
    }
}
