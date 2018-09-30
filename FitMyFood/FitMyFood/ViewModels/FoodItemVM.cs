using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;


namespace FitMyFood.ViewModels
{
    public class FoodItemVM : BaseVM, Architecture.FoodItemVMI
    {
        public Command VariationItem_SaveCommand { get; set; }
        public Command VariationItem_CancelCommand { get; set; }
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

        public FoodItemVM(INavigation navigation, FoodItem foodItem, Variation variation) : base(navigation)
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
            VariationItem_SaveCommand = new Command(async () => await ExecuteSaveCommand());
            VariationItem_CancelCommand = new Command(async () => await ExecuteCancelCommand());
        }

        async Task ExecuteSaveCommand()
        {
            IsBusy = true;
            if (newitem)
            {
                await App.DB.AddNewVariationFoodItemAsync(Item.Quantity, Variation, Item);
            }
            await App.DB.SaveChangesAsync();
            App.VariationItemVM.Item = Item;
            //App.VariationItemVM.Weight = Item.Weight;
            IsBusy = false;
            await Navigation.PopModalAsync();
        }
        async Task ExecuteCancelCommand()
        {
            await Navigation.PopModalAsync();
        }
    }
}
