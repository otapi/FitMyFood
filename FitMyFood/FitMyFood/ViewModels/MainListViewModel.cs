using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;
using System.Collections.Generic;
using FitMyFood.Services;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers;
using System.Linq;


// TODO: convert all .Wait() and .Result to:
/* Task.Run(async () =>
      {
            await ExecuteLoadSelectorsCommand();
await PopulateVariationSelector();
        });
*/
namespace FitMyFood.ViewModels
{
    public class MainListViewModel : BaseViewModel
    {
        public static double ENERGYFAT = 9.3;
        public static double ENERGYPROTEIN = 4.2;
        public static double ENERGYCARBO = 4.1;
        public static double ENERGYBODYFAT = 7;

        public static double SEDENTARY = 1.2;
        public static double MODERATELY = 1.3;
        public static double ACTIVE = 1.4;

        public ObservableRangeCollection<FoodItem> Items { get; set; }
        public ObservableRangeCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableRangeCollection<View> MealSelectorSource { get; set; }
        public ObservableRangeCollection<View> VariationSelectorSource { get; set; }

        List<DailyProfile> dailyProfileSelectors;
        List<Meal> mealSelectors;
        List<Variation> variationSelectors;

        public Settings Settings { get; set; }
        FoodItem _TargetFood;
        public FoodItem TargetFood
        {
            get { return _TargetFood; }
            set { SetProperty(ref _TargetFood, value); }
        }
        FoodItem _TotalFood;
        public FoodItem TotalFood
        {
            get { return _TotalFood; }
            set { SetProperty(ref _TotalFood, value); }
        }
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

                VariationItem_EditCommand.Execute(null);
                
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


        int _DailyProfileSelectorIndex;
        public int DailyProfileSelectorIndex
        {
            set
            {
                SetProperty(ref _DailyProfileSelectorIndex, value);
                if (dailyProfileSelectors.Count > 0 && DailyProfileSelectorIndex>-1)
                {
                    DailyProfile = dailyProfileSelectors[DailyProfileSelectorIndex];
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
                if (mealSelectors.Count > 0 && MealSelectorIndex > -1)
                {
                    Meal = mealSelectors[MealSelectorIndex];
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
                if (variationSelectors.Count > 0 && VariationSelectorIndex > -1)
                {
                    MealVariation = variationSelectors[VariationSelectorIndex];
                }
                App.MainListViewModel.LoadItemsCommand.Execute(null);
            }
            get
            {
                return _VariationSelectorIndex;
            }
        }

        FoodDiagramModel _EnergyDiagram;
        public FoodDiagramModel EnergyDiagram
        {
            set
            {
                SetProperty(ref _EnergyDiagram, value);
            }
            get
            {
                return _EnergyDiagram;
            }
        }

        public IAsyncCommand LoadSelectorsCommand { get; set; } 
        public IAsyncCommand UpdateVariantSelectorCommand { get; set; }
        public IAsyncCommand LoadItemsCommand { get; set; }
        public IAsyncCommand VariationItem_EditCommand { get; set; }
        public IAsyncCommand VariationItem_NewCommand { get; private set; }


        void DefineCommands()
        {
            LoadSelectorsCommand = new AsyncCommand(ExecuteLoadSelectorsCommand);
            LoadItemsCommand = new AsyncCommand(ExecuteLoadItemsCommand);
            UpdateVariantSelectorCommand = new AsyncCommand(PopulateVariationSelector);
            VariationItem_EditCommand = new AsyncCommand(ExecuteVariationItem_EditCommand);
            VariationItem_NewCommand = new AsyncCommand(ExecuteVariationItem_NewCommand);
           
        }
        public MainListViewModel()
        {
            App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] start");

            Title = "Browse";
            DefineCommands();

            Items = new ObservableRangeCollection<FoodItem>();
            DailyProfileSelectorSource = new ObservableRangeCollection<View>();
            MealSelectorSource = new ObservableRangeCollection<View>();
            VariationSelectorSource = new ObservableRangeCollection<View>();

            dailyProfileSelectors = new List<DailyProfile>();
            mealSelectors = new List<Meal>();
            variationSelectors = new List<Variation>();
            
            TargetFood = new FoodItem();
            TotalFood = new FoodItem();

            Settings = App.DB.GetSettings().Result;

            LoadSelectorsCommand.Execute(null);
        }

        /*According to Nutrition Therapy and Pathophysiology, the Mifflin-St. Jeor equation was
         developed in 1990 and has been validated by more than 10 studies. The Mifflin-St. Jeor
         equation is gaining popularity among the nutrition professionals for accurately estimating
         caloric needs. The equation is as follows:
         for females = 10 x (Weight in kg) + 6.25 x (Height in cm) - 5 x age - 161;
         for males= 10 x (Weight in kg) + 6.25 x (Height in cm) - 5 x age + 5.

         These equations are also multiplied by the same physical activity factors to estimate
         daily caloric needs.
        */
        void CalcTargetFood()
        {
            string message = "";
            double BMR;
            if (Settings.Sex == false)
            {
                message = "Female\n";
                BMR = 10 * Settings.ActualWeight + 6.25 * Settings.Height - 5 * Settings.Age - 161;
            }
            else
            {
                message = "Male\n";
                BMR = 10 * Settings.ActualWeight + 6.25 * Settings.Height - 5 * Settings.Age + 5;
            }
            message += $"Weight: {Settings.ActualWeight }\n" +
                        $"Height: {Settings.Height}\n" +
                        $"Age: {Settings.Age}\n";

            double physicalActivity = 0;
            switch (Settings.Physical_activity)
            {
                case 1:
                    physicalActivity = 1.1;
                    break;
                case 2:
                    physicalActivity = 1.2;
                    break;
                case 3:
                    physicalActivity = 1.3;
                    break;
            }
            double dailyKcal = BMR * physicalActivity + DailyProfile.ExtraKcal;
            double dailyWeightChangeInGramm = Settings.WeeklyWeightChange * 1000 / ENERGYBODYFAT;

            // 7 is just an estimated number of body energy...
            double dailyKcalChange = dailyWeightChangeInGramm * ENERGYBODYFAT;

            double dailyKcalTarget = dailyKcal + dailyKcalChange;
            double mealKcal = dailyKcalTarget * Meal.KcalRatio / 100;

            int fatPercent = Settings.DailyFatRatio;
            int carboPercent = Settings.DailyCarboRatio;
            int proteinPercent = Settings.DailyProteinRatio;

            double fatKcal = mealKcal * fatPercent / 100;
            double carboKcal = mealKcal * carboPercent / 100;
            double proteinKcal = mealKcal * proteinPercent / 100;

            double fat = fatKcal / ENERGYFAT;
            double carbo = carboKcal / ENERGYCARBO;
            double protein = proteinKcal / ENERGYPROTEIN;

            TargetFood = new FoodItem()
            {
                Fat = fat,
                Carbo = carbo,
                Protein = protein,
                Quantity = 1,
                UnitWeight = 100
            };

            message += $"\nBMR: {BMR} kcal\n" +
                        $"Maintenance: {dailyKcal} kcal\n" +
                        $"Target change: {dailyKcalChange} kcal\n" +
                        $"Target weight change: {dailyWeightChangeInGramm} gramm\n" +
                        $"Target energy: {dailyKcalTarget} kcal\n";
        }
        void CalcSummary()
        {
            TotalFood = new FoodItem
            {
                Fat = 0,
                Carbo = 0,
                Protein = 0,
                Quantity = 1,
                UnitWeight = 100
            };

            foreach (var food in Items)
            {
                TotalFood.Fat += food.Fat * food.Weight/100;
                TotalFood.Carbo += food.Carbo * food.Weight/100;
                TotalFood.Protein += food.Protein * food.Weight/100;
            }
            if (TargetFood == null || TargetFood.Energy == 0)
            {
                CalcTargetFood();
            }
            SummaryEnergy = $"Energy: {TotalFood.Energy:0.#} / {TargetFood.Energy:0.#} kcal";
            SummaryFat = $"Fat: {TotalFood.Fat:0.#} / {TargetFood.Fat:0.#} gramm";
            SummaryCarbo = $"Carbo: {TotalFood.Carbo:0.#} / {TargetFood.Carbo:0.#} gramm";
            SummaryProtein = $"Protein: {TotalFood.Protein:0.#} / {TargetFood.Protein:0.#} gramm";

            EnergyDiagram = new FoodDiagramModel()
            {
                Type = FoodDiagramModel.Types.Energy,
                Actual = 5,
                //Actual = TotalFood.Energy,
                Maximum = TargetFood.Energy,
                WholeDay = TargetFood.Energy*4 // TODO: calc the real whole day energy
            };
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
            mealSelectors = await App.DB.GetMealsAsync();
            MealSelectorSource.Clear();
            MealSelectorSource.AddRange(
                from item in mealSelectors
                select new Label()
                {
                    Text = item.Name, HorizontalTextAlignment = TextAlignment.Center
                }
            );

            MealSelectorIndex = 0;
        }

        async Task PopulateDailyProfileSelector()
        {
            dailyProfileSelectors = await App.DB.GetDailyProfilesAsync();
            DailyProfileSelectorSource.Clear();
            DailyProfileSelectorSource.AddRange(
                from item in dailyProfileSelectors
                select new Label()
                {
                    Text = item.Name, HorizontalTextAlignment = TextAlignment.Center
                }
            );
            DailyProfileSelectorIndex = 0;
        }

        async Task PopulateVariationSelector()
        {

            if (mealSelectors.Count <1 || dailyProfileSelectors.Count < 1 || DailyProfile == null || Meal == null)
            {
                return;
            }
            IsBusy = true;

            variationSelectors = await App.DB.GetVariationsAsync(DailyProfile, Meal);
            VariationSelectorSource.Clear();
            VariationSelectorSource.AddRange(
                from item in variationSelectors
                select new Label()
                {
                    Text = item.Name, HorizontalTextAlignment = TextAlignment.Center
                }
            );
            
            MealVariation = variationSelectors[VariationSelectorIndex];
            CalcTargetFood();
            IsBusy = false;
        }
        /// <summary>
        /// This is ist
        /// </summary>
        /// <returns></returns>
        async Task ExecuteLoadItemsCommand()
        {
            if (MealVariation == null)
            {
                return;
            }
            IsBusy = true;
            Items.Clear();
            var variationFoodItems = await App.DB.GetVariationFoodItemsIncludeFoodItem(MealVariation);
            var itemsToAdd = new List<FoodItem>();
            foreach (var variationFoodItem in variationFoodItems)
            {
                var foodItem = await App.DB.GetFoodItemAsNoTracked(variationFoodItem.FoodItem);
                foodItem.Quantity = variationFoodItem.Quantity;
                itemsToAdd.Add(foodItem);
            }
            Items.ReplaceRange(itemsToAdd);
            CalcSummary();
            
            IsBusy = false;
        }
        
        async Task ExecuteVariationItem_EditCommand()
        {
            await App.Navigation.PushAsync(new VariationItemPage(SelectedItem, MealVariation));
        }
        async Task ExecuteVariationItem_NewCommand()
        {
            await App.Navigation.PushAsync(new VariationItemPage(null, MealVariation));
        }

        

    }
}