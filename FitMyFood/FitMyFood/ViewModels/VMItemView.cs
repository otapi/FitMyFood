using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class VMItemView : VMBase
    {
        INavigation navigation { get; set; }

        public Command EditFoodItemDetailCommand { get; set; }
        public FoodItem Item { get; set; }
        public VMItemView(INavigation navigation, FoodItem foodItem)
        {
            this.navigation = navigation;
            Item = foodItem;
            EditFoodItemDetailCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());
        }

        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new NavigationPage(new ItemEditPage(Item)));
            IsBusy = false;
        }
    }
}
