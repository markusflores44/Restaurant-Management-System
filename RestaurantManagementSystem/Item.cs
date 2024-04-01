﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    public class Item
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private double _price;
        public double Price
        {
            get => _price;
            set => _price = value;
        }

        public Item(string paramName, double paramPrice)
        {
            this.Name = paramName;
            this.Price = paramPrice;
        }
    }
}
