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
            public string SummaryEnergy { get; set; }
            public string SummaryFat { get; set; }
            public string SummaryCarbo { get; set; }
            public string SummaryProtein { get; set; }

        }
        public static double ENERGYFAT = 9.3;
        public static double ENERGYPROTEIN = 4.2;
        public static double ENERGYCARBO = 4.1;
        public static double ENERGYBODYFAT = 7;

        public ObservableCollection<FoodItem> Items { get; set; }
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        public FoodItem TargetFood { get; set; }
        public FoodItem TotalFood { get; set; }
        FoodItem _SelectedItem;
        public FoodItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                SetProperty(ref _SelectedItem, value);
                
                if (_SelectedItem == null)
                    return;

                ViewFoodItemDetailCommand.Execute(null);
                
                SelectedItem = null;
            }
        }

        string _summaryEnergy = string.Empty;
        public string SummaryEnergy
        {
            get { return _summaryEnergy; }
            set { SetProperty(ref _summaryEnergy, value); }
        }
        string _summaryFat = string.Empty;
        public string SummaryFat
        {
            get { return _summaryFat; }
            set { SetProperty(ref _summaryFat, value); }
        }
        string _summaryCarbo = string.Empty;
        public string SummaryCarbo
        {
            get { return _summaryCarbo; }
            set { SetProperty(ref _summaryCarbo, value); }
        }
        string _summaryProtein = string.Empty;
        public string SummaryProtein
        {
            get { return _summaryProtein; }
            set { SetProperty(ref _summaryProtein, value); }
        }
        
        Meal Meal { get; set; }
        DailyProfile DailyProfile { get; set; }
        // TODO: rename to Variation
        public Variation MealVariation { get; set; }

        List<DailyProfile> DailyProfileSelectorItems;
        List<Meal> MealSelectorItems;
        List<Variation> VariationSelectorItems;

        int _DailyProfileSelectorIndex;
        public int DailyProfileSelectorIndex
        {
            set
            {
                SetProperty(ref _DailyProfileSelectorIndex, value);
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
                SetProperty(ref _MealSelectorIndex, value);
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
                SetProperty(ref _VariationSelectorIndex, value);
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
        public Command ViewFoodItemDetailCommand { get; set; }


        void DefineCommands()
        {
            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            UpdateVariantSelectorCommand = new Command(async () => await PopulateVariationSelector());
            ViewFoodItemDetailCommand = new Command(async () => await ExecuteViewFoodItemViewCommand());
            
        }
        public MainListFoodItemVM(INavigation navigation) : base(navigation)
        {
            Title = "Browse";
            DefineCommands();

            Items = new ObservableCollection<FoodItem>();
            DailyProfileSelectorSource = new ObservableCollection<View>();
            MealSelectorSource = new ObservableCollection<View>();
            VariationSelectorSource = new ObservableCollection<View>();

            DailyProfileSelectorItems = new List<DailyProfile>();
            MealSelectorItems = new List<Meal>();
            VariationSelectorItems = new List<Variation>();
            
            TargetFood = new FoodItem();
            TotalFood = new FoodItem();

        }

        void CalcTargetFood()
        {
            // TODO: implement
        }
        void CalcSummary()
        {
            TotalFood = new FoodItem
            {
                Fat = 0,
                Carbo = 0,
                Protein = 0
            };

            foreach (var food in Items)
            {
                TotalFood.Fat += food.Fat * food.Weight/100;
                TotalFood.Carbo += food.Carbo * food.Weight/100;
                TotalFood.Protein += food.Protein * food.Weight/100;
            }
            SummaryEnergy = $"Energy: {TotalFood.Energy} / {TargetFood.Energy} kcal";
            SummaryFat = $"Fat: {TotalFood.Energy} / {TargetFood.Energy} gramm";
            SummaryCarbo = $"Carbo: {TotalFood.Energy} / {TargetFood.Energy} gramm";
            SummaryProtein = $"Protein: {TotalFood.Energy} / {TargetFood.Energy} gramm";
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
            MealSelectorItems = await App.DB.GetMealsAsync();
            MealSelectorSource.Clear();
            foreach (var item in MealSelectorItems)
            {
                MealSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
        }

        async Task PopulateDailyProfileSelector()
        {
            DailyProfileSelectorItems = await App.DB.GetDailyProfilesAsync();
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

            VariationSelectorItems = await App.DB.GetVariationsAsync(DailyProfile, Meal);
            VariationSelectorSource.Clear();
            foreach (var item in VariationSelectorItems)
            {
                VariationSelectorSource.Add(new Label() { Text = item.Name, HorizontalTextAlignment = TextAlignment.Center });
            }
            MealVariation = VariationSelectorItems[VariationSelectorIndex];
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (MealVariation == null)
            {
                return;
            }
            IsBusy = true;
            Items.Clear();

            var variationFoodItems = await App.DB.GetVariationFoodItemsIncludeFoodItem(MealVariation);
            foreach (var variationFoodItem in variationFoodItems)
            {
                var foodItem = variationFoodItem.FoodItem;
                foodItem.Quantity = variationFoodItem.Quantity;
                Items.Add(foodItem);
            }
            CalcSummary();
            IsBusy = false;
        }
        
        async Task ExecuteViewFoodItemViewCommand()
        {
            if (SelectedItem == null)
            {
                return;
            }
            // TODO: cache it to avoid repetative DB get
            var variationFoodItem = await App.DB.GetVariationFoodItemAsync(SelectedItem, MealVariation);
            await Navigation.PushAsync(new ItemViewPage(SelectedItem, variationFoodItem));
        }

        
    }
}