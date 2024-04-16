using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem
{
    public abstract class BillTemplate
    {
        public abstract void AddItem(Item newItem, int newQuantity);
    }
}
