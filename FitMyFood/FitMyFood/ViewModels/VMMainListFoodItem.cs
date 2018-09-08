using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;
using System.Collections.Generic;

namespace FitMyFood.ViewModels
{
    public class VMMainListFoodItem : VMBase
    {
        public ObservableCollection<FoodItem> Items { get; set; }
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        int _DailyProfileSelectorIndex;
        public int DailyProfileSelectorIndex
        {
            set
            {
                _DailyProfileSelectorIndex = value;
                OnPropertyChanged("DailyProfileSelectorIndex");
                UpdateVariantSelectorCommand.Execute(null);
            }
            get
            {
                return _DailyProfileSelectorIndex;
            }
        }
        int _MealSelectorIndex;
        public int MealSelectorIndex
        {
            set
            {
                _MealSelectorIndex = value;
                OnPropertyChanged("MealSelectorIndex");
                UpdateVariantSelectorCommand.Execute(null);
            }
            get
            {
                return _MealSelectorIndex;
            }
        }
        public int VariationSelectorindex { get; set; }

        public Command LoadSelectorsCommand { get; set; }
        public Command UpdateVariantSelectorCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        List<DailyProfile> DailyProfileSelectorItems;
        List<Meal> MealSelectorItems;
        List<DailyProfileMealVariation> VariationSelectorItems;


        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            DailyProfileSelectorSource = new ObservableCollection<View>();
            MealSelectorSource = new ObservableCollection<View>();
            VariationSelectorSource = new ObservableCollection<View>();

            DailyProfileSelectorItems = new List<DailyProfile>();
            MealSelectorItems = new List<Meal>();
            VariationSelectorItems = new List<DailyProfileMealVariation>();

            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemCommand(foodItem));
            UpdateVariantSelectorCommand = new Command(async () => await PopulateVariationSelector());

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
            MealSelectorItems = await App.dataStore.meals.GetItemsAsync();
            if (MealSelectorItems.Count == 0)
            {
                MealSelectorItems.Add(new Meal() { Name = "Breakfast", KcalRatio = 20 });
                MealSelectorItems.Add(new Meal() { Name = "1st Snack", KcalRatio = 10 });
                MealSelectorItems.Add(new Meal() { Name = "Lunch", KcalRatio = 35 });
                MealSelectorItems.Add(new Meal() { Name = "2nd Snack", KcalRatio = 10 });
                MealSelectorItems.Add(new Meal() { Name = "Dinner", KcalRatio = 25 });
                await App.dataStore.meals.AddItemsAsync(MealSelectorItems);
            }

            MealSelectorSource.Clear();
            foreach (var item in MealSelectorItems)
            {
                MealSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateDailyProfileSelector()
        {
            DailyProfileSelectorItems = await App.dataStore.dailyProfiles.GetItemsAsync();
            if (DailyProfileSelectorItems.Count == 0)
            {
                DailyProfileSelectorItems.Add(new DailyProfile() { Name = "Normal", ExtraKcal = 0 });
                DailyProfileSelectorItems.Add(new DailyProfile() { Name = "Sport", ExtraKcal = 800 });
                await App.dataStore.dailyProfiles.AddItemsAsync(DailyProfileSelectorItems);
            }

            DailyProfileSelectorSource.Clear();
            foreach (var item in DailyProfileSelectorItems)
            {
                DailyProfileSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateVariationSelector()
        {
            if (MealSelectorItems.Count <1 || DailyProfileSelectorItems.Count < 1)
            {
                return;
            }
            
            Meal meal = MealSelectorItems[MealSelectorIndex];
            DailyProfile dailyProfile = DailyProfileSelectorItems[DailyProfileSelectorIndex];

            VariationSelectorItems = await App.dataStore.GetVariationsAsync(dailyProfile, meal);
            if (VariationSelectorItems.Count == 0)
            {
                VariationSelectorItems.Add(new DailyProfileMealVariation() {Name="Variation A", DailyProfileId = dailyProfile.Id, MealId = meal.Id});
                await App.dataStore.dailyProfileMealVariation.AddItemsAsync(VariationSelectorItems);
            }

            VariationSelectorSource.Clear();
            foreach (var item in VariationSelectorItems)
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