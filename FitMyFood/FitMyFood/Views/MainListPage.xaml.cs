﻿using System;
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
            App.MainListVM = new MainListVM(Navigation);
            App.MenuVM.Navigation = Navigation;
            BindingContext = App.MainListVM;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainListVM.LoadItemsCommand.Execute(null);
        }

        
    }
}