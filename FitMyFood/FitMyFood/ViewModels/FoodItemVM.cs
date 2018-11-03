using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

// TODO: selectall text edits on focus with a Renderer or an Effect

namespace FitMyFood.ViewModels
{
    public class FoodItemVM : BaseVM
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
                foodItem = new FoodItem();
            } else
            {
                newitem = false;
            }

            this.Item = foodItem;
            Variation = variation;
            VariationItem_SaveCommand = new Command(async () => await VariationItem_Save());
            VariationItem_CancelCommand = new Command(async () => await VariationItem_Cancel());
        }

        async Task VariationItem_Save()
        {
            IsBusy = true;
            VariationItemSearchItem var = new VariationItemSearchItem()
            {
                Name = Item.Name,
                InternalFoodItem = Item
            };
            if (newitem)
            {
                await App.DB.AddFoodItem(Item);
                await App.DB.SaveChangesAsync();
            }
            App.VariationItemVM.SearchItems.Add(var);
            App.VariationItemVM.SelectedSearchItem = var;
            IsBusy = false;
            await Navigation.PopAsync(true);
        }
        async Task VariationItem_Cancel()
        {
            await Navigation.PopAsync(true);
        }
    }
}
