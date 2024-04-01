using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    public class Customer
    {
        private string _name;
        public string Name
        {
            get => _name; 
            set => _name = value;
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => _phoneNumber = value;
        }

        public Customer(string paramName, string paramPhone)
        {
            this.Name = paramName;
            this.PhoneNumber = paramPhone;
        }
    }
}
