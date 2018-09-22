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
        
        FoodItem _Item;
        public FoodItem Item
        {
            get { return _Item; }
            set { SetProperty(ref _Item, value);
                
            }
        }
        public VMItemView(INavigation navigation, FoodItem foodItem)
        {
            this.navigation = navigation;
            Item = foodItem;
            EditFoodItemDetailCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());

            MessagingCenter.Subscribe<VMItemEdit, FoodItem>(this, "ChangeItem", async (obj, item) =>
            {
                Item = item;
            });
        }

        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await navigation.PushModalAsync(new NavigationPage(new ItemEditPage(Item)));
            IsBusy = false;
        }

        public void changeQuantity()
        {
            App.vmMainListFoodItem.SaveFoodItemForVariationCommand.Execute(Item);
        }
    }
}
