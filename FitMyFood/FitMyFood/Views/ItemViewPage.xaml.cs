using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemViewPage : ContentPage
    {
        public ItemViewPage(VMItemDetail viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        public ItemViewPage()
        {
            InitializeComponent();
        }
    }
}