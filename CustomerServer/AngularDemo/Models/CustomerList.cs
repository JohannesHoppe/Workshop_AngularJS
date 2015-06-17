using System.Collections.Generic;

namespace AngularDemo.Models
{
    public class CustomerList
    {
        private static readonly List<Customer> _customers = new List<Customer>(); 

        public List<Customer> Customers
        {
            get { return _customers; } 
        }

        public void SaveChanges()
        {
            
        }
    }
}