using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(VMItemDetail viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();
        }
    }
}