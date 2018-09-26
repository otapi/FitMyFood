using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListFoodItemPage : ContentPage
    {
        public MainListFoodItemPage()
        {
            InitializeComponent();
            // TODO: make it a command?
            Task<SQLiteAsyncConnection> task = Task.Run(() => Data.DataStore.GetDataStore());
            task.Wait();
            App.DB = task.Result;
            
            App.MainListFoodItemVM = new MainListFoodItemVM(Navigation);
            BindingContext = App.MainListFoodItemVM;
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as FoodItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new VMItemDetail(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
            */
        }

        void OnStepperChanged(object sender, EventArgs e)
        {
            FoodItem foodItem = (sender as Stepper).BindingContext as FoodItem;
            App.MainListFoodItemVM.ItemStepperChangedCommand.Execute(foodItem);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainListFoodItemVM.LoadSelectorsCommand.Execute(null);
        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.MainListFoodItemVM;
            App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
        }

       
    }
}