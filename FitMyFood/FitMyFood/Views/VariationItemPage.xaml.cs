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

            App.VariationItemVM = new VariationItemVM(Navigation, foodItem, variation);
            BindingContext = App.VariationItemVM;
        }

        void OnQuantityChanged(object sender, EventArgs e)
        {
            Editor quant = (sender as Editor);
            if (quant.Text != null)
            {
                App.VariationItemVM.ChangeQuantity().Wait();
            }
        }
    }
}