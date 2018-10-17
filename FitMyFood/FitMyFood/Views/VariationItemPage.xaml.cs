using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VariationItemPage : ContentPage
    {
        public VariationItemPage(FoodItem foodItem, Variation variation)
        {
            InitializeComponent();

            App.VariationItemVM = new VariationItemVM(Navigation, foodItem, variation);
            BindingContext = App.VariationItemVM;
            if (foodItem == null)
            {
                App.VariationItemVM.FillSearchFoodItemsCommand.Execute(null);
            }
        }

        async void OnQuantityChanged(object sender, EventArgs e)
        {
            Entry quant = (sender as Entry);
            if (quant.Text != null)
            {
                await App.VariationItemVM.ChangeQuantity();
            }
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchItemsListview.BeginRefresh();
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                App.VariationItemVM.FillSearchFoodItemsCommand.Execute(null);
            else
                App.VariationItemVM.FillSearchFoodItemsCommand.Execute(e.NewTextValue);

            SearchItemsListview.EndRefresh();
        }

        protected override bool OnBackButtonPressed()
        {
            if (App.VariationItemVM.IsSearchItemsListviewVisible)
            {
                App.VariationItemVM.IsSearchItemsListviewVisible = false;
            } else
            {
                App.VariationItemVM.MainList_EditFinishedCommand.Execute(null);
            }
            
            return true;
        }
    }
}