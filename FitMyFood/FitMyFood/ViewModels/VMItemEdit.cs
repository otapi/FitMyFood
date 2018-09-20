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
        FoodItem _Item;
        public FoodItem Item {
            get
            {
                
                return _Item;
            }
            set
            {
                Title = value?.Name;
                _Item = value;
            }
        }

        public VMItemEdit(INavigation navigation, FoodItem foodItem)
        {
            this.navigation = navigation;
            if (foodItem == null)
            {
                foodItem = Data.DefaultValues.FoodItem();
            }

            this.Item = foodItem;
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
            CancelCommand = new Command(async () => await ExecuteCancelCommand());
        }

        async Task ExecuteSaveCommand()
        {
            IsBusy = true;
            MessagingCenter.Send(this, "AddItem", Item);
            IsBusy = false;
            await navigation.PopModalAsync();
        }
        async Task ExecuteCancelCommand()
        {
            await navigation.PopModalAsync();
        }
    }
}
