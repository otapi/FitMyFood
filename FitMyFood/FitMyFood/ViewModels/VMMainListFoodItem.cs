using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class VMMainListFoodItem : VMBase
    {
        public ObservableCollection<FoodItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemCommand(foodItem));
            
            MessagingCenter.Subscribe<NewItemPage, FoodItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as FoodItem;
                Items.Add(newItem);
                await App.dataStore.foodItems.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
                Items.Clear();
                var items = await App.dataStore.foodItems.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            IsBusy = false;
        }
        async Task ExecuteSaveFoodItemCommand(FoodItem foodItem)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            await App.dataStore.foodItems.UpdateItemAsync(foodItem);
            IsBusy = false;
        }

    }
}