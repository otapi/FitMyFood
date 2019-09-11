﻿using FitMyFood.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FitMyFood.Views;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;

namespace FitMyFood.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public MasterDetailPage MasterDetailPage { get; set; }

        public MenuViewModel(INavigation navigation) : base(navigation)
        {

        }

        public enum MenuItemType
        {
            WeightTrack,
            Settings,
            About
        }

        public List<HomeMenuItem> GetMenuItems()
        {
            return new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.WeightTrack, Title="Weights" },
                new HomeMenuItem {Id = MenuItemType.Settings, Title="Settings" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };
        }

        public async Task NavigateTo(MenuItemType page)
        {
            switch (page)
            {
                case MenuItemType.WeightTrack:
                    await Navigation.PushAsync(new WeightTrackPage());
                    break;
                case MenuItemType.Settings:
                    await Navigation.PushAsync(new SettingsPage());
                    break;
                case MenuItemType.About:
                    await Navigation.PushAsync(new AboutPage());
                    break;
            }
            MasterDetailPage.IsPresented = false;
        }

        
    }
}
