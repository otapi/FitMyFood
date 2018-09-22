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
        public ItemViewPage(FoodItem foodItem)
        {
            InitializeComponent();

            BindingContext = new VMItemView(Navigation, foodItem);
        }

        void OnQuantityChanged(object sender, EventArgs e)
        {
            Editor quant = (sender as Editor);
            if (quant.Text != null)
            {
                (BindingContext as VMItemView).changeQuantity();
            }
        }
    }
}