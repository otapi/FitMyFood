using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.Views;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListFoodItemPage : ContentPage
    {
        public MainListFoodItemPage()
        {
            InitializeComponent();

            BindingContext = App.MainListFoodItemViewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as FoodItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App.MainListFoodItemViewModel.Items.Count == 0)
                App.MainListFoodItemViewModel.LoadItemsCommand.Execute(null);
        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.MainListFoodItemViewModel;
        }

        void OnStepperChanged(object sender, EventArgs e)
        {
            FoodItem foodItem = (sender as Xamarin.Forms.Stepper).BindingContext as Models.FoodItem;
            new Common.Commands.SaveFoodItem().Execute(foodItem);
        }
    }
}