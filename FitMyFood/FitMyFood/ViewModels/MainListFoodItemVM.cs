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
    public class MainListFoodItemVM : BaseVM
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
        public Variation MealVariation { get; set; }

        List<DailyProfile> DailyProfileSelectorItems;
        List<Meal> MealSelectorItems;
        List<Variation> VariationSelectorItems;

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
        public Command ItemStepperChangedCommand { get; set; }
        public Command ViewFoodItemDetailCommand { get; set; }
        public Command RemoveItemFromMainList { get; set; }
        public Command AddItemPageCommand { get; set; }

        void defineCommands()
        {
            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemStepperChangedCommand = new Command<FoodItem>(async (foodItem) => await ExecuteItemStepperChangedCommand(foodItem));
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
            VariationSelectorItems = new List<Variation>();
            
            targetFood = new FoodItem();
            totalFood = new FoodItem();

        }

        void calcTargetFood()
        {
            // TODO: implement
        }
        void calcSummary()
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
            MealSelectorItems = await App.DB.getMealsAsync();
            MealSelectorSource.Clear();
            foreach (var item in MealSelectorItems)
            {
                MealSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateDailyProfileSelector()
        {
            DailyProfileSelectorItems = await App.DB.getDailyProfilesAsync();
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

            VariationSelectorItems = await App.DB.getVariationsAsync(DailyProfile, Meal);
            VariationSelectorSource.Clear();
            foreach (var item in VariationSelectorItems)
            {
                VariationSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
            MealVariation = VariationSelectorItems[VariationSelectorIndex];
            LoadItemsCommand.Execute(null);
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (MealVariation == null)
            {
                return;
            }
            IsBusy = true;
            Items.Clear();

            var variationFoodItems = await App.DB.getVariationFoodItemsNoTrackingAsync(MealVariation);
            foreach (var variationFoodItem in variationFoodItems)
            {
                var foodItem = variationFoodItem.FoodItem;
                foodItem.Quantity = variationFoodItem.Quantity;
                Items.Add(foodItem);
            }
            calcSummary();
            IsBusy = false;
        }
        async Task ExecuteItemStepperChangedCommand(FoodItem foodItem)
        {
            if (foodItem == null || MealVariation == null)
            {
                return;
            }
            // TODO: cache it to avoid repetative DB get
            var variationFoodItem = await App.DB.getVariationFoodItem(foodItem, MealVariation);
            variationFoodItem.Quantity = foodItem.Quantity;
            await App.DB.updateQuantityOnVariationFoodItem(variationFoodItem);
            calcSummary();
        }

        async Task ExecuteViewFoodItemViewCommand(FoodItem foodItem)
        {
            // TODO: PushModalAsync?
            await Navigation.PushAsync(new ItemViewPage(foodItem));
            await ExecuteLoadItemsCommand();
        }

        async Task ExecuteRemoveItemFromMainListCommand(FoodItem foodItem)
        {
            IsBusy = true;
            var variationFoodItem = await App.DB.GetAllWithChildrenAsync<VariationFoodItem>(v => v.FoodItem == foodItem);

            if (variationFoodItem.Count == 0)
            {
                App.PrintWarning($"You want to remove it, but the reference for food ({foodItem.Id}) is already missing.");
                IsBusy = false;
                return;
            }

            if (variationFoodItem.Count > 1)
            {
                App.PrintWarning($"More than one references found for food ({foodItem.Id}).");
            }

            await App.DB.DeleteAllAsync(variationFoodItem);
            IsBusy = false;
        }
        async Task ExecuteAddItemPageCommand()
        {
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(null)));
            await ExecuteLoadItemsCommand();
        }
    }
}