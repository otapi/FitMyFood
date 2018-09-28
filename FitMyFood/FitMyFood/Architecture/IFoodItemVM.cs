using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FitMyFood.Architecture
{
    interface FoodItemVMI
    {
        Command VariationItem_SaveCommand { get; set; }
        Command VariationItem_CancelCommand { get; set; }
    }
}
