using FitMyFood.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = App.MenuViewModel.GetMenuItems();

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                await App.MenuViewModel.NavigateTo(((HomeMenuItem)e.SelectedItem).Id);
                ListViewMenu.SelectedItem = null;
                
            };
            
        }
    }
}