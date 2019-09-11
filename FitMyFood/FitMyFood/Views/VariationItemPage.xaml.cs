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

            App.VariationItemViewModel = new VariationItemViewModel(Navigation, foodItem, variation);
            BindingContext = App.VariationItemViewModel;
            if (foodItem == null)
            {
                App.VariationItemViewModel.FillSearchFoodItemsCommand.Execute(null);
            }
        }

        async void OnQuantityChanged(object sender, EventArgs e)
        {
            Entry quant = (sender as Entry);
            if (quant.Text != null)
            {
                await App.VariationItemViewModel.ChangeQuantity();
            }
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchItemsListview.BeginRefresh();
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                App.VariationItemViewModel.FillSearchFoodItemsCommand.Execute(null);
            else
                App.VariationItemViewModel.FillSearchFoodItemsCommand.Execute(e.NewTextValue);

            SearchItemsListview.EndRefresh();
        }

        protected override bool OnBackButtonPressed()
        {
            if (App.VariationItemViewModel.IsSearchItemsListviewVisible)
            {
                App.VariationItemViewModel.IsSearchItemsListviewVisible = false;
            } else
            {
                App.VariationItemViewModel.MainList_EditFinishedCommand.Execute(null);
            }
            
            return true;
        }
    }
}