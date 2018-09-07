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
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        View _dailyProfileSelectorPosition;
        public View DailyProfileSelectorPosition
        {
            set
            {
                _dailyProfileSelectorPosition = value;
                OnPropertyChanged("DailyProfileSelectorPosition");
                PopulateVariationSelector().Wait();
            }
            get
            {
                return _dailyProfileSelectorPosition;
            }
        }
        View _mealSelectorPosition;
        public View MealSelectorPosition
        {
            set
            {
                _mealSelectorPosition = value;
                OnPropertyChanged("MealSelectorPosition");
                PopulateVariationSelector().Wait();
            }
            get
            {
                return _mealSelectorPosition;
            }
        }
        
        public View VariationSelectorPosition { get; set; }

        public Command LoadSelectorsCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            DailyProfileSelectorSource = new ObservableCollection<View>();
            MealSelectorSource = new ObservableCollection<View>();
            VariationSelectorSource = new ObservableCollection<View>();

            DailyProfileSelectorPosition = new Label();
            MealSelectorPosition = new Label();
            VariationSelectorPosition = new Label();


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
                items.Add(new DailyProfile() { Name = "Normal", ExtraKcal = 0 });
                items.Add(new DailyProfile() { Name = "Sport", ExtraKcal = 800 });
                await App.dataStore.dailyProfiles.AddItemsAsync(items);
            }

            DailyProfileSelectorSource.Clear();
            foreach (var item in items)
            {
                DailyProfileSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateVariationSelector()
        {
            if (MealSelectorPosition == null || DailyProfileSelectorPosition == null
                || (MealSelectorPosition as Label).Text == null || (DailyProfileSelectorPosition as Label).Text == null)
            {
                return;
            }
            string mealName = (MealSelectorPosition as Label).Text;
            string dailyProfileName = (DailyProfileSelectorPosition as Label).Text;

            Meal meal = await App.dataStore.meals.GetFirstItemByNameAsync(mealName);
            DailyProfile dailyProfile = await App.dataStore.dailyProfiles.GetFirstItemByNameAsync(dailyProfileName);
            
            if (meal == null || dailyProfile == null)
            {
                App.PrintWarning("Could not find Meal ("+mealName+") or DailyProfile("+dailyProfileName);
                return;
            }
            var items = await App.dataStore.GetVariationsAsync(dailyProfile, meal);
            if (items.Count == 0)
            {
                items.Add(new DailyProfileMealVariation() {Name="Variation A", DailyProfileId = dailyProfile.Id, MealId = meal.Id});
                await App.dataStore.dailyProfileMealVariation.AddItemsAsync(items);
            }

            VariationSelectorSource.Clear();
            foreach (var item in items)
            {
                VariationSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
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