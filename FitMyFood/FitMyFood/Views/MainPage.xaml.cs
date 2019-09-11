using FitMyFood.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;
            App.MenuViewModel.MasterDetailPage = this;
            
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
        }

        
    }
}