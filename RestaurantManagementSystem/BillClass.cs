using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    // implements abstract class Bill Template
    public class BillClass : BillTemplate
    {
        private List<Item> _itemList;
        public List<Item> ItemList
        {
            get => _itemList; 
            set => _itemList = value;
        }

        private List<int> _quantityList;
        public List<int> QuantityList
        {
            get => _quantityList;
            set => _quantityList = value;
        }

        public BillClass() 
        {
            _itemList = new List<Item>();
            _quantityList = new List<int>();
        }

        //add item functionality with checking if it already exists
        public override void AddItem(Item newItem, int newQuantity)
        {
            if (this.ItemList.Contains(newItem))
            {
                int itemIndex = this.ItemList.FindIndex(i => i == newItem);
                this.QuantityList[itemIndex] += newQuantity;
            }
            else
            {
                this.ItemList.Add(newItem);
                this.QuantityList.Add(newQuantity);
            }
            
        }
    }
}
