using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class VMItemDetail : VMBase
    {
        public INavigation navigation { get; set; }

        public Command EditFoodItemDetailCommand { get; set; }
        public FoodItem Item { get; set; }
        public VMItemDetail(FoodItem item = null)
        {
            if (item == null)
            {
                item = Data.DefaultValues.FoodItem;
            }
            Title = item?.Name;
            Item = item;
        }

        // TODO: convert to commands
        /*
        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        */
        async Task ExecuteEditFoodItemDetailCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await navigation.PushAsync(new ItemEditPage(this));
            IsBusy = false;
        }
    }
}
