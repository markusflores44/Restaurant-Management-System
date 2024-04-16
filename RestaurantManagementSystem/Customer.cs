﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    //Creates and edits the Customer
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

        public Customer(int paramNum ,string paramName, string paramPhone)
        {
            this.Customer_Num = paramNum;
            this.Name = paramName;
            this.PhoneNumber = paramPhone;

        }

        // Parameterless constructor allows object initializers
        public Customer() { }

        private int _customer_num;
        public int Customer_Num
        {
            get => _customer_num;
            set => _customer_num = value;
        }

        //Displays the data in the selected. 
        public string FullDetails => $"ID: {Customer_Num}, Name: {_name}, Phone: {_phoneNumber}";
    }
}
