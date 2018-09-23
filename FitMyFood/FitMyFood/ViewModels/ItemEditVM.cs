using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class ItemEditVM : BaseVM
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
                await App.MainListFoodItemVM.AddNewItem(Item);
            } else
            {
                await App.DataStore.foodItems.SaveItemAsync(Item);
                App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
                App.ItemViewVM.ChangeItem(Item);
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
