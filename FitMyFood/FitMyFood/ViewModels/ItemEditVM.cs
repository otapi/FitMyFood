using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;


namespace FitMyFood.ViewModels
{
    public class ItemEditVM : BaseVM, Architecture.IItemEdit
    {
        public Command SaveCommand { get; set; }
        public Command CancelCommand { get; set; }
        public bool newitem;
        FoodItem _Item;
        public FoodItem Item {
            get
            {
                
                return _Item;
            }
            set
            {
                Title = value?.Name;
                SetProperty(ref _Item, value);

            }
        }

        public ItemEditVM(INavigation navigation, FoodItem foodItem) : base(navigation)
        {
            if (foodItem == null)
            {
                newitem = true;
                foodItem = Data.DefaultValues.FoodItem();
            } else
            {
                newitem = false;
            }

            this.Item = foodItem;
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            CancelCommand = new Command(async () => await ExecuteCancelCommand());
        }

        async Task ExecuteSaveCommand()
        {
            IsBusy = true;
            if (newitem)
            {
                // Came from the MainList - Insert new FoodItem to DB
                // and go back to the MainList - so that needs to be updated as well
                var variationFoodItem = new VariationFoodItem()
                {
                    Quantity = Item.Quantity,
                };
                await App.DB.InsertAsync(variationFoodItem);

                App.MainListFoodItemVM.MealVariation.VariationFoodItems.Add(variationFoodItem);
                await App.DB.UpdateWithChildrenAsync(variationFoodItem);

                Item.VariationFoodItems.Add(variationFoodItem);
                await App.DB.InsertOrReplaceWithChildrenAsync(Item);
                App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
            }
            else
            {
                // Came from the ItemView, FoodItem already existed
                await App.DB.InsertOrReplaceWithChildrenAsync(Item);
                App.ItemViewVM.Item = Item;
            }
            IsBusy = false;
            await Navigation.PopModalAsync();
        }
        async Task ExecuteCancelCommand()
        {
            await Navigation.PopModalAsync();
        }
    }
}
