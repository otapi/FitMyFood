﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.Views;
using FitMyFood.ViewModels;
using System.Collections.ObjectModel;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListFoodItemPage : ContentPage
    {
        public MainListFoodItemPage()
        {
            InitializeComponent();

            BindingContext = App.vmMainListFoodItem;
            App.vmMainListFoodItem.navigation = Navigation;
            App.vmMainListFoodItem.LoadSelectorsCommand.Execute(null);
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
            if (foodItem != null)
            {
                App.vmMainListFoodItem.SaveFoodItemCommand.Execute(foodItem);
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(new VMItemDetail())));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.vmMainListFoodItem;
            App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
        }

        public void OnEdit(object sender, EventArgs e)
        {
            ViewCell aa = new ViewCell();
            
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }

    }
}