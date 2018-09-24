using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Models;

namespace FitMyFood.Architecture
{
    interface IMainListFoodItem
    {
        Command LoadSelectorsCommand { get; set; }
        Command UpdateVariantSelectorCommand { get; set; }
        Command LoadItemsCommand { get; set; }
        Command SaveFoodItemForVariationCommand { get; set; }
        Command ViewFoodItemDetailCommand { get; set; }
        Command RemoveItemFromMainList { get; set; }
        Command AddItemPageCommand { get; set; }

        Task AddNewItemAsync(FoodItem item);
    }
}
