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
            App.Navigation = Navigation;
            App.MainListViewModel = new MainListViewModel();
            BindingContext = App.MainListViewModel;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainListViewModel.LoadItemsCommand.Execute(null);
        }

        
    }
}