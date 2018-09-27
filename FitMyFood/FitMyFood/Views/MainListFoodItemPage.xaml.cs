using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.ViewModels;


namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListFoodItemPage : ContentPage
    {
        public MainListFoodItemPage()
        {
            InitializeComponent();
            App.DB = new Data.DatabaseHelper();
            App.MainListFoodItemVM = new MainListFoodItemVM(Navigation);
            BindingContext = App.MainListFoodItemVM;
            
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