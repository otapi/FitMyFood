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
        public INavigation navigation { get; set; }
        public ObservableCollection<FoodItem> Items { get; set; }
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        Meal Meal { get; set; }
        DailyProfile DailyProfile { get; set; }
        DailyProfileMealVariation MealVariation { get; set; }

        List<DailyProfile> DailyProfileSelectorItems;
        List<Meal> MealSelectorItems;
        List<DailyProfileMealVariation> VariationSelectorItems;

        int _DailyProfileSelectorIndex;
        public int DailyProfileSelectorIndex
        {
            set
            {
                _DailyProfileSelectorIndex = value;
                OnPropertyChanged("DailyProfileSelectorIndex");
                if (DailyProfileSelectorItems.Count > 0 && DailyProfileSelectorIndex>-1)
                {
                    DailyProfile = DailyProfileSelectorItems[DailyProfileSelectorIndex];
                }
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
                if (MealSelectorItems.Count > 0 && MealSelectorIndex > -1)
                {
                    Meal = MealSelectorItems[MealSelectorIndex];
                }
                UpdateVariantSelectorCommand.Execute(null);
            }
            get
            {
                return _MealSelectorIndex;
            }
        }
        int _VariationSelectorindex;
        public int VariationSelectorindex
        {
            set
            {
                _VariationSelectorindex = value;
                OnPropertyChanged("VariationSelectorindex");
                if (VariationSelectorItems.Count > 0 && VariationSelectorindex > -1)
                {
                    MealVariation = VariationSelectorItems[VariationSelectorindex];
                }
                App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
            }
            get
            {
                return _VariationSelectorindex;
            }
        }


        public Command LoadSelectorsCommand { get; set; }
        public Command UpdateVariantSelectorCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }
        public Command ViewFoodItemDetailCommand { get; set; }
        public Command DeleteFoodItemCommand { get; set; }

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

            defineCommands();

            MessagingCenter.Subscribe<NewItemPage, FoodItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = await App.dataStore.foodItems.SaveItemAsync(item);
                await App.dataStore.SaveFoodItemForVariation(DailyProfile, Meal, MealVariation, newItem);
                App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
            });
        }

        void defineCommands()
        {
            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemCommand(foodItem));
            UpdateVariantSelectorCommand = new Command(async () => await PopulateVariationSelector());
            ViewFoodItemDetailCommand = new Command<FoodItem>(async (foodItem) => await ExecuteViewFoodItemDetailCommand(foodItem));
            DeleteFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteDeleteFoodItemCommand(foodItem));
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
                await App.dataStore.meals.SaveItemsAsync(MealSelectorItems);
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
                await App.dataStore.dailyProfiles.SaveItemsAsync(DailyProfileSelectorItems);
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
            
            VariationSelectorItems = await App.dataStore.GetVariationsAsync(DailyProfile, Meal);
            if (VariationSelectorItems.Count == 0)
            {
                VariationSelectorItems.Add(new DailyProfileMealVariation() {Name="Variation A", DailyProfileId = DailyProfile.Id, MealId = Meal.Id});
                await App.dataStore.dailyProfileMealVariation.SaveItemsAsync(VariationSelectorItems);
            }

            VariationSelectorSource.Clear();
            foreach (var item in VariationSelectorItems)
            {
                VariationSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
            MealVariation = VariationSelectorItems[VariationSelectorindex];
            App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            Items.Clear();
            var items = await App.dataStore.GetFoodItemsForMainList(DailyProfile, Meal, MealVariation);
            foreach (var item in items)
            {
                Items.Add(item);
            }
            IsBusy = false;
        }
        async Task ExecuteSaveFoodItemCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await App.dataStore.foodItems.SaveItemAsync(foodItem);
            IsBusy = false;
        }

        async Task ExecuteViewFoodItemDetailCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await navigation.PushAsync(new ItemDetailPage(new VMItemDetail(foodItem)));
            //await ExecuteLoadItemsCommand();
            IsBusy = false;
        }
        async Task ExecuteDeleteFoodItemCommand(FoodItem foodItem)
        {
            IsBusy = true;
            // TODO

            await ExecuteLoadItemsCommand();
            IsBusy = false;
        }
    }
}