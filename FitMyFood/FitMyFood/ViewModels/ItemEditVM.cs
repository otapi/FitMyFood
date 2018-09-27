using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;


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
        public Variation Variation { get; set; }

        public ItemEditVM(INavigation navigation, FoodItem foodItem, Variation variation) : base(navigation)
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
            Variation = variation;
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
                await App.DB.AddNewVariationFoodItemAsync(Item.Quantity, Variation, Item);
                App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
            }
            else
            {
                // Came from the ItemView, FoodItem already existed
                await App.DB.SaveChangesAsync();
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
