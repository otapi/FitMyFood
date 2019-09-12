using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers;

// TODO: selectall text edits on focus with a Renderer or an Effect

namespace FitMyFood.ViewModels
{
    public class FoodItemViewModel : BaseViewModel
    {
        public IAsyncCommand VariationItem_SaveCommand { get; set; }
        public IAsyncCommand VariationItem_CancelCommand { get; set; }
        public bool isNewitem;
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

        public FoodItemViewModel(FoodItem foodItem, Variation variation)
        {
            if (foodItem == null)
            {
                isNewitem = true;
                foodItem = new FoodItem();
            } else
            {
                isNewitem = false;
            }

            this.Item = foodItem;
            Variation = variation;
            VariationItem_SaveCommand = new AsyncCommand(VariationItem_Save);
            VariationItem_CancelCommand = new AsyncCommand(VariationItem_Cancel);
        }

        // just for XAML Intellisense
        public FoodItemViewModel()
        {

        }
        async Task VariationItem_Save()
        {
            IsBusy = true;
            VariationItemSearchItem var = new VariationItemSearchItem()
            {
                Name = Item.Name,
                InternalFoodItem = Item
            };
            if (isNewitem)
            {
                await App.DB.AddFoodItem(Item);
                await App.DB.SaveChangesAsync();
            }
            App.VariationItemViewModel.SearchItems.Add(var);
            App.VariationItemViewModel.SelectedSearchItem = var;
            IsBusy = false;
            await App.Navigation.PopAsync(true);
        }
        async Task VariationItem_Cancel()
        {
            await App.Navigation.PopAsync(true);
        }
    }
}
