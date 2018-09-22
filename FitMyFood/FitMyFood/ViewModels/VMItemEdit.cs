using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class VMItemEdit : VMBase
    {
        INavigation navigation { get; set; }

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

        public VMItemEdit(INavigation navigation, FoodItem foodItem)
        {
            this.navigation = navigation;
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
                MessagingCenter.Send(this, "AddItem", Item);
            } else
            {
                await App.dataStore.foodItems.SaveItemAsync(Item);
                App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
                MessagingCenter.Send(this, "ChangeItem", Item);
            }
            IsBusy = false;
            await navigation.PopModalAsync();
        }
        async Task ExecuteCancelCommand()
        {
            await navigation.PopModalAsync();
        }
    }
}
