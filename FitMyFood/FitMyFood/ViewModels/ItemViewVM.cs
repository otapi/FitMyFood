using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using SQLiteNetExtensionsAsync.Extensions;

namespace FitMyFood.ViewModels
{
    public class ItemViewVM : BaseVM, Architecture.IItemView
    {
        public Command EditFoodItemDetailCommand { get; set; }
        
        FoodItem _Item;
        public FoodItem Item
        {
            get { return _Item; }
            set { SetProperty(ref _Item, value);
                
            }
        }

        VariationFoodItem VariationFoodItem;
        public ItemViewVM(INavigation navigation, FoodItem foodItem) : base(navigation)
        {
            Item = foodItem;
            EditFoodItemDetailCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());
        }

        async Task getVariationFoodItem()
        {
            var variationFoodItems = await App.DB.GetAllWithChildrenAsync<VariationFoodItem>(v => v.FoodItem == Item);

            if (variationFoodItems.Count == 0)
            {
                App.PrintWarning($"Zero references found for food ({Item.Id}).");
                return;
            }

            if (variationFoodItems.Count > 1)
            {
                App.PrintWarning($"More than one references found for food ({Item.Id}).");
            }
            VariationFoodItem = variationFoodItems[0];
        }

        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(Item)));
            IsBusy = false;
        }

        // TODO: convert this to an async Command
        public void changeQuantity()
        {
            if (VariationFoodItem == null)
            {
                getVariationFoodItem().Wait();
            }
            VariationFoodItem.Quantity = Item.Quantity;
            App.DB.InsertOrReplaceAsync(VariationFoodItem).Wait();
        }
    }
}
