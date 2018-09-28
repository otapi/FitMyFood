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
    public partial class MainListPage : ContentPage
    {
        public MainListPage()
        {
            InitializeComponent();
            App.DB = new Data.DatabaseHelper();
            App.MainListVM = new MainListVM(Navigation);
            BindingContext = App.MainListVM;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainListVM.LoadSelectorsCommand.Execute(null);
        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.MainListVM;
            App.MainListVM.LoadItemsCommand.Execute(null);
        }      
    }
}