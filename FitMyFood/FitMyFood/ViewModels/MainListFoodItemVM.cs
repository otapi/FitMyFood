using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;
using System.Collections.Generic;
using FitMyFood.Data;

namespace FitMyFood.ViewModels
{
    public class MainListFoodItemVM : BaseVM, Architecture.IMainListFoodItem
    {
        public class Summary
        {
            public string summaryEnergy { get; set; }
            public string summaryFat { get; set; }
            public string summaryCarbo { get; set; }
            public string summaryProtein { get; set; }

        }
        public static double ENERGYFAT = 9.3;
        public static double ENERGYPROTEIN = 4.2;
        public static double ENERGYCARBO = 4.1;
        public static double ENERGYBODYFAT = 7;

        public ObservableCollection<FoodItem> Items { get; set; }
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        public FoodItem targetFood { get; set; }
        public FoodItem totalFood { get; set; }

        string _summaryEnergy = string.Empty;
        public string summaryEnergy
        {
            get { return _summaryEnergy; }
            set { SetProperty(ref _summaryEnergy, value); }
        }
        string _summaryFat = string.Empty;
        public string summaryFat
        {
            get { return _summaryFat; }
            set { SetProperty(ref _summaryFat, value); }
        }
        string _summaryCarbo = string.Empty;
        public string summaryCarbo
        {
            get { return _summaryCarbo; }
            set { SetProperty(ref _summaryCarbo, value); }
        }
        string _summaryProtein = string.Empty;
        public string summaryProtein
        {
            get { return _summaryProtein; }
            set { SetProperty(ref _summaryProtein, value); }
        }
        
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
        int _VariationSelectorIndex;
        public int VariationSelectorIndex
        {
            set
            {
                _VariationSelectorIndex = value;
                OnPropertyChanged("VariationSelectorindex");
                if (VariationSelectorItems.Count > 0 && VariationSelectorIndex > -1)
                {
                    MealVariation = VariationSelectorItems[VariationSelectorIndex];
                }
                App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
            }
            get
            {
                return _VariationSelectorIndex;
            }
        }


        public Command LoadSelectorsCommand { get; set; }
        public Command UpdateVariantSelectorCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemForVariationCommand { get; set; }
        public Command ViewFoodItemDetailCommand { get; set; }
        public Command RemoveItemFromMainList { get; set; }
        public Command AddItemPageCommand { get; set; }

        void defineCommands()
        {
            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemForVariationCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemForVariationCommand(foodItem));
            UpdateVariantSelectorCommand = new Command(async () => await PopulateVariationSelector());
            ViewFoodItemDetailCommand = new Command<FoodItem>(async (foodItem) => await ExecuteViewFoodItemViewCommand(foodItem));
            RemoveItemFromMainList = new Command<FoodItem>(async (foodItem) => await ExecuteRemoveItemFromMainListCommand(foodItem));
            AddItemPageCommand = new Command(async () => await ExecuteAddItemPageCommand());

        }
        public MainListFoodItemVM(INavigation navigation) : base(navigation)
        {
            Title = "Browse";
            defineCommands();

            Items = new ObservableCollection<FoodItem>();
            DailyProfileSelectorSource = new ObservableCollection<View>();
            MealSelectorSource = new ObservableCollection<View>();
            VariationSelectorSource = new ObservableCollection<View>();

            DailyProfileSelectorItems = new List<DailyProfile>();
            MealSelectorItems = new List<Meal>();
            VariationSelectorItems = new List<DailyProfileMealVariation>();
            
            targetFood = new FoodItem();
            totalFood = new FoodItem();

        }

        async public Task AddNewItemAsync(FoodItem item)
        {
            var newItem = await App.DataStore.foodItems.SaveItemAsync(item);
            await App.DataStore.SaveFoodItemForVariation(DailyProfile, Meal, MealVariation, newItem);
            LoadItemsCommand.Execute(null);
        }
        void calcTargetFood()
        {
            // TODO: implement
        }
        public void calcSummary()
        {
            totalFood = new FoodItem();
            totalFood.Fat = 0;
            totalFood.Carbo = 0;
            totalFood.Protein = 0;

            foreach (var food in Items)
            {
                totalFood.Fat += food.Fat * food.Weight/100;
                totalFood.Carbo += food.Carbo * food.Weight/100;
                totalFood.Protein += food.Protein * food.Weight/100;
            }
            summaryEnergy = $"Energy: {totalFood.Energy} / {targetFood.Energy} kcal";
            summaryFat = $"Fat: {totalFood.Energy} / {targetFood.Energy} gramm";
            summaryCarbo = $"Carbo: {totalFood.Energy} / {targetFood.Energy} gramm";
            summaryProtein = $"Protein: {totalFood.Energy} / {targetFood.Energy} gramm";
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
            MealSelectorItems = await App.DataStore.meals.GetItemsAsync();
            if (MealSelectorItems.Count == 0)
            {
                MealSelectorItems.AddRange(DefaultValues.MealSelectorItems);
                await App.DataStore.meals.SaveItemsAsync(MealSelectorItems);
            }

            MealSelectorSource.Clear();
            foreach (var item in MealSelectorItems)
            {
                MealSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateDailyProfileSelector()
        {
            DailyProfileSelectorItems = await App.DataStore.dailyProfiles.GetItemsAsync();
            if (DailyProfileSelectorItems.Count == 0)
            {
                DailyProfileSelectorItems.AddRange(DefaultValues.DailyProfileSelectorItems);
                await App.DataStore.dailyProfiles.SaveItemsAsync(DailyProfileSelectorItems);
            }

            DailyProfileSelectorSource.Clear();
            foreach (var item in DailyProfileSelectorItems)
            {
                DailyProfileSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateVariationSelector()
        {
            if (MealSelectorItems.Count <1 || DailyProfileSelectorItems.Count < 1 || DailyProfile == null || Meal == null)
            {
                return;
            }
            
            VariationSelectorItems = await App.DataStore.GetVariationsAsync(DailyProfile, Meal);
            if (VariationSelectorItems.Count == 0)
            {
                VariationSelectorItems.Add(new DailyProfileMealVariation() {Name=DefaultValues.VariationSelectorItem, DailyProfileId = DailyProfile.Id, MealId = Meal.Id});
                await App.DataStore.dailyProfileMealVariation.SaveItemsAsync(VariationSelectorItems);
            }

            VariationSelectorSource.Clear();
            foreach (var item in VariationSelectorItems)
            {
                VariationSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
            MealVariation = VariationSelectorItems[VariationSelectorIndex];
            App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (DailyProfile == null || Meal == null || MealVariation == null)
            {
                return;
            }
            IsBusy = true;
            Items.Clear();
            var items = await App.DataStore.GetFoodItemsForMainList(DailyProfile, Meal, MealVariation);
            foreach (var item in items)
            {
                Items.Add(item);
            }
            calcSummary();
            IsBusy = false;
        }
        async Task ExecuteSaveFoodItemForVariationCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await App.DataStore.SaveFoodItemForVariation(DailyProfile, Meal, MealVariation, foodItem);
            IsBusy = false;
        }

        async Task ExecuteViewFoodItemViewCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await Navigation.PushAsync(new ItemViewPage(foodItem));
            App.MainListFoodItemVM.LoadItemsCommand.Execute(null);
            IsBusy = false;
        }
        async Task ExecuteRemoveItemFromMainListCommand(FoodItem foodItem)
        {
            IsBusy = true;
            await App.DataStore.RemoveFoodItemsFromMainList(DailyProfile, Meal, MealVariation, foodItem);
            Items.Remove(foodItem);
            IsBusy = false;
        }
        async Task ExecuteAddItemPageCommand()
        {
            IsBusy = true;
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(null)));
            IsBusy = false;
        }
    }
}