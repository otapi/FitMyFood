using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class VariationItemVM : BaseVM, Architecture.VariationItemVMI
    {
        public Command FoodItem_EditCommand { get; set; }
        public Command MainList_RemoveItemCommand { get; set; }
        public Command FoodItem_NewCommand { get; set; }
        public Command MainList_EditFinishedCommand { get; set; }

        FoodItem _Item;
        public FoodItem Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);
            }
        }

        
        public Variation Variation;
        VariationFoodItem VariationFoodItem;

        public VariationItemVM(INavigation navigation, FoodItem foodItem, Variation variation) : base(navigation)
        {
            Item = foodItem;
            FoodItem_EditCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());
            MainList_RemoveItemCommand = new Command(async () => await ExecuteRemoveItemFromMainListCommand());
            FoodItem_NewCommand = new Command(async () => await ExecuteFoodItem_NewCommand());
            MainList_EditFinishedCommand = new Command(async () => await ExecuteMainList_EditFinishedCommand());

            Variation = variation;
        }

        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new FoodItemPage(Item, null)));
            IsBusy = false;
        }

        async Task ExecuteFoodItem_NewCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new FoodItemPage(null, Variation)));
            IsBusy = false;
        }

        // TODO: convert this to a behavior or a property change
        public async Task ChangeQuantity()
        {
            if (Item == null)
            {
                return;
            }
            if (VariationFoodItem == null)
            {
                VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            }
            VariationFoodItem.Quantity = Item.Quantity;
            App.DB.SaveChangesAsync().Wait();
        }

        async Task ExecuteRemoveItemFromMainListCommand()
        {
            if (Item == null)
            {
                return;
            }
            if (VariationFoodItem == null)
            {
                VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            }
            VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            await App.DB.RemoveVariationFoodItemAsync(VariationFoodItem);
            IsBusy = false;
            await Navigation.PopAsync(true);
        }
        async Task ExecuteMainList_EditFinishedCommand()
        {
            await Navigation.PopAsync(true);
        }

    }
}
