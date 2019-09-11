﻿using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;

// TODO: selectall text edits on focus with a Renderer or an Effect

namespace FitMyFood.ViewModels
{
    public class FoodItemViewModel : BaseViewModel
    {
        public IAsyncCommand VariationItem_SaveCommand { get; set; }
        public IAsyncCommand VariationItem_CancelCommand { get; set; }
        public bool newitem;
        FoodItem _Item;
        public FoodItem Item {
            get
            {
                
                return _Item;
            }
            set
            {
                Title = value?.Name;
                SetProperty(ref _Item, value);

            }
        }
        public Variation Variation { get; set; }

        public FoodItemViewModel(INavigation navigation, FoodItem foodItem, Variation variation) : base(navigation)
        {
            if (foodItem == null)
            {
                newitem = true;
                foodItem = new FoodItem();
            } else
            {
                newitem = false;
            }

            this.Item = foodItem;
            Variation = variation;
            VariationItem_SaveCommand = new AsyncCommand(VariationItem_Save);
            VariationItem_CancelCommand = new AsyncCommand(VariationItem_Cancel);
        }

        async Task VariationItem_Save()
        {
            IsBusy = true;
            VariationItemSearchItem var = new VariationItemSearchItem()
            {
                Name = Item.Name,
                InternalFoodItem = Item
            };
            if (newitem)
            {
                await App.DB.AddFoodItem(Item);
                await App.DB.SaveChangesAsync();
            }
            App.VariationItemViewModel.SearchItems.Add(var);
            App.VariationItemViewModel.SelectedSearchItem = var;
            IsBusy = false;
            await Navigation.PopAsync(true);
        }
        async Task VariationItem_Cancel()
        {
            await Navigation.PopAsync(true);
        }
    }
}