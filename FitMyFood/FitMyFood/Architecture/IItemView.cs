using System;
using System.Collections.Generic;
using System.Text;
using FitMyFood.Models;

namespace FitMyFood.Architecture
{
    interface IItemView
    {

        void ChangeItem(FoodItem item);
        void changeQuantity();
    }
}
