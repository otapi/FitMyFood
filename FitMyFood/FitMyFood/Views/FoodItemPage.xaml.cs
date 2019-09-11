using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodItemPage : ContentPage
    {
        public FoodItemPage(FoodItem foodItem, Variation variation)
        {
            InitializeComponent();
            App.FoodItemViewModel = new FoodItemViewModel(foodItem, variation);
            BindingContext = App.FoodItemViewModel;
        }

    }
}