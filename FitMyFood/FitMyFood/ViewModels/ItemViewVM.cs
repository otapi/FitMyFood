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
            set { SetProperty(ref _Item, value);
                
            }
        }
        public ItemViewVM(INavigation navigation, FoodItem foodItem) : base(navigation)
        {
            Item = foodItem;
            EditFoodItemDetailCommand = new Command(async () => await ExecuteEditFoodItemDetailCommand());
        }

        public void ChangeItem(FoodItem item)
        {
            Item = item;
        }
        async Task ExecuteEditFoodItemDetailCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(Item)));
            IsBusy = false;
        }

        public void changeQuantity()
        {
            App.MainListFoodItemVM.SaveFoodItemForVariationCommand.Execute(Item);
        }
    }
}
