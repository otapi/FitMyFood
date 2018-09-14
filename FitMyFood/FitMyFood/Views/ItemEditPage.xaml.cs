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
        public ItemEditPage(VMItemDetail viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

        }
    }
}