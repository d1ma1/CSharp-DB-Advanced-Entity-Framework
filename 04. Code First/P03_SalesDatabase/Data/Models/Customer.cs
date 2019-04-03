using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {

        public Customer()
        {
            Sales = new List<Sale>();
        }
        public ICollection<Sale> Sales { get; set; }

        public int	CustomerId { get; set; }
        public string	Name { get; set; }
        public string	Email { get; set; }
        public string	CreditCardNumber { get; set; }

    }
}
