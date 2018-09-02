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
        public ObservableCollection<View> DailProfileItemSource { get; set; }


        public Command LoadSelectorsCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            DailProfileItemSource = new ObservableCollection<View>();

            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemCommand(foodItem));
            

            MessagingCenter.Subscribe<NewItemPage, FoodItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as FoodItem;
                Items.Add(newItem);
                await App.dataStore.foodItems.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadSelectorsCommand()
        {
            IsBusy = true;
            
            var items = await App.dataStore.dailyProfiles.GetItemsAsync();
            if (items.Count == 0)
            {
                items.Add(new DailyProfile() { Name = "Normal", ExtraKcal = 0});
                items.Add(new DailyProfile() { Name = "Sport", ExtraKcal = 800 });
            }
            await App.dataStore.dailyProfiles.AddItemsAsync(items);

            DailProfileItemSource.Clear(); // = new ObservableCollection<View>();
            foreach (var item in items)
            {
                DailProfileItemSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
            IsBusy = false;
        }
        async Task ExecuteLoadItemsCommand()
        {
            

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
            
            IsBusy = true;

            await App.dataStore.foodItems.UpdateItemAsync(foodItem);
            IsBusy = false;
        }

    }
}