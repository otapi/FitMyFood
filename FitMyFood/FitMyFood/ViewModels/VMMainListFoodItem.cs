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
        public ObservableCollection<View> DailProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }
        

        public Command LoadSelectorsCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            DailProfileSelectorSource = new ObservableCollection<View>();
            MealSelectorSource = new ObservableCollection<View>();
            VariationSelectorSource = new ObservableCollection<View>();


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
            await PopulateDailyProfileSelector();
            await PopulateMealSelector();
            IsBusy = false;
                    }

        async Task PopulateMealSelector()
        {
            var items = await App.dataStore.meals.GetItemsAsync();
            if (items.Count == 0)
            {
                items.Add(new Meal() { Name = "Breakfast", KcalRatio = 20 });
                items.Add(new Meal() { Name = "1st Snack", KcalRatio = 10 });
                items.Add(new Meal() { Name = "Lunch", KcalRatio = 35 });
                items.Add(new Meal() { Name = "2nd Snack", KcalRatio = 10 });
                items.Add(new Meal() { Name = "Dinner", KcalRatio = 25 });
                await App.dataStore.meals.AddItemsAsync(items);
            }

            MealSelectorSource.Clear();
            foreach (var item in items)
            {
                MealSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateDailyProfileSelector()
        {
            var items = await App.dataStore.dailyProfiles.GetItemsAsync();
            if (items.Count == 0)
            {
                items.Add(new DailyProfile() { DailyProfileName = "Normal", ExtraKcal = 0 });
                items.Add(new DailyProfile() { DailyProfileName = "Sport", ExtraKcal = 800 });
                await App.dataStore.dailyProfiles.AddItemsAsync(items);
            }

            DailProfileSelectorSource.Clear(); // = new ObservableCollection<View>();
            foreach (var item in items)
            {
                DailProfileSelectorSource.Add(new Label() { Text = item.DailyProfileName, HorizontalTextAlignment = TextAlignment.Center });
            }
            await Task.CompletedTask;

        }

        async Task PopulateVariationSelector()
        {
            var items = await App.dataStore.dailyProfileMealVariation.GetItemsAsync();
            if (items.Count == 0)
            {
                items.Add(new DailyProfileMealVariation() { VariationName DailyProfileName = "Variation A", ExtraKcal = 0 });
                items.Add(new DailyProfile() { DailyProfileName = "Sport", ExtraKcal = 800 });
                await App.dataStore.dailyProfiles.AddItemsAsync(items);
            }

            DailProfileSelectorSource.Clear(); // = new ObservableCollection<View>();
            foreach (var item in items)
            {
                DailProfileSelectorSource.Add(new Label() { Text = item.DailyProfileName, HorizontalTextAlignment = TextAlignment.Center });
            }
            await Task.CompletedTask;

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
            await Task.CompletedTask;

        }
        async Task ExecuteSaveFoodItemCommand(FoodItem foodItem)
        {
            
            IsBusy = true;

            await App.dataStore.foodItems.UpdateItemAsync(foodItem);
            IsBusy = false;
            await Task.CompletedTask;
        }

    }
}