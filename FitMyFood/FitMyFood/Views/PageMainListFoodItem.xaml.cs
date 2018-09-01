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
using CarouselView.FormsPlugin.Abstractions;
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
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.vmMainListFoodItem;
        }

        
    }
}