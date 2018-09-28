using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FitMyFood.Models;
using Xamarin.Forms;
namespace FitMyFood.Architecture
{
    interface VariationItemVMI
    {
        Command FoodItem_NewCommand { get; set; }
        Command FoodItem_EditCommand { get; set; }
        Command MainList_RemoveItemCommand { get; set; }
        Command MainList_EditFinishedCommand { get; set; }
    }
}
