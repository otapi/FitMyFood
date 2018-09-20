using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemEditPage : ContentPage
    {
        public ItemEditPage(FoodItem foodItem)
        {
            InitializeComponent();

            BindingContext = new VMItemEdit(Navigation, foodItem);
        }

    }
}