using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class ItemViewVM : BaseVM, Architecture.IItemView
    {
        public Command EditFoodItemDetailCommand { get; set; }

        FoodItem _Item;
        public FoodItem Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);

            }
        }

        VariationFoodItem VariationFoodItem;
        public ItemViewVM(INavigation navigation, FoodItem foodItem, VariationFoodItem variationFoodItem) : base(navigation)
        {
            Item = foodItem;
            EditFoodItemDetailCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());
            VariationFoodItem = variationFoodItem;
        }

        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(Item, null)));
            IsBusy = false;
        }

        // TODO: convert this to a behavior or a property change
        public void changeQuantity()
        {
            VariationFoodItem.Quantity = Item.Quantity;
            App.DB.SaveChangesAsync().Wait();

        }
    }
}
