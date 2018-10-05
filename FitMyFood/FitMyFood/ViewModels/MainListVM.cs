using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;
using System.Collections.Generic;
using FitMyFood.Data;

/*
    FitMyFod
    com.otapigems.fitmyfood
    Copyright (C) 2018 Barnabás Nagy - otapiGems.com - otapiGems@protonmail.ch
   
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace FitMyFood.ViewModels
{
    public class MainListVM : BaseVM
    {
        public static double ENERGYFAT = 9.3;
        public static double ENERGYPROTEIN = 4.2;
        public static double ENERGYCARBO = 4.1;
        public static double ENERGYBODYFAT = 7;

        public static double SEDENTARY = 1.2;
        public static double MODERATELY = 1.3;
        public static double ACTIVE = 1.4;

        public ObservableCollection<FoodItem> Items { get; set; }
        public ObservableCollection<View> DailyProfileSelectorSource { get; set; }
        public ObservableCollection<View> MealSelectorSource { get; set; }
        public ObservableCollection<View> VariationSelectorSource { get; set; }

        public Settings Settings { get; set; }
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
                App.MainListVM.LoadItemsCommand.Execute(null);
            }
            get
            {
                return _VariationSelectorIndex;
            }
        }


        public Command LoadSelectorsCommand { get; set; } 
        public Command UpdateVariantSelectorCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command VariationItem_EditCommand { get; set; }
        public Command VariationItem_NewCommand { get; set; }


        void DefineCommands()
        {
            LoadSelectorsCommand = new Command(async () => await ExecuteLoadSelectorsCommand());
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            UpdateVariantSelectorCommand = new Command(async () => await PopulateVariationSelector());
            VariationItem_EditCommand = new Command(async () => await ExecuteVariationItem_EditCommand());
            VariationItem_NewCommand = new Command(async () => await ExecuteVariationItem_NewCommand());
            
        }
        public MainListVM(INavigation navigation) : base(navigation)
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

            var t = App.DB.GetSettings();
            t.Wait();
            Settings = t.Result;

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
             public double BMR;
        public double dailyKcal;
        public double dailyKcalChange;
        public double dailyKcalTarget;
        public double dailyWeightChangeInGramm;
        int extraKcal = S
            int mealKcalRatio
                if (settings.isSex() == Sex.FEMALE)
                {
                    message = "Female\n";
                    BMR = 10 * settings.getActualWeight() + 6.25 * settings.getHeight() - 5 * settings.getAge() - 161;
                }
                else
                {
                    message = "Male\n";
                    BMR = 10 * settings.getActualWeight() + 6.25 * settings.getHeight() - 5 * settings.getAge() + 5;
                }
                message += String.format(
                        "Weight: %d\n" +
                        "Height: %d\n" +
                        "Age: %d\n",
                        (int)settings.getActualWeight(),
                        settings.getHeight(),
                    settings.getAge());
                dailyKcal = BMR * settings.getPhysical_activity() + extraKcal;
                dailyWeightChangeInGramm = settings.getWeeklyWeightChange() * 1000 / ENERGYBODYFAT;

                // 7 is just an estimated number of body energy...
                dailyKcalChange = dailyWeightChangeInGramm * ENERGYBODYFAT;

                dailyKcalTarget = dailyKcal + dailyKcalChange;
                double mealKcal = dailyKcalTarget * mealKcalRatio / 100;

                int fatPercent = settings.getDailyFatRatio();
                int carboPercent = settings.getDailyCarboRatio();
                int proteinPercent = settings.getDailyProteinRatio();

                double fatKcal = mealKcal * fatPercent / 100;
                double carboKcal = mealKcal * carboPercent / 100;
                double proteinKcal = mealKcal * proteinPercent / 100;

                double fat = fatKcal / ENERGYFAT;
                double carbo = carboKcal / ENERGYCARBO;
                double protein = proteinKcal / ENERGYPROTEIN;

                FoodItem targetFoodItem = new FoodItem("Target", 100, fat, carbo, protein);

                message += String.format(
                                "\nBMR: %d kcal\n" +
                                "Maintenance: %d kcal\n" +
                                "Target change: %d\n" +
                                "Target weight change: %d gramm\n" +
                                "Target energy: %d kcal\n",
                        (int)BMR,
                        (int)dailyKcal,
                        (int)dailyKcalChange,
                        (int)dailyWeightChangeInGramm,
                        (int)dailyKcalTarget);
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
            SummaryEnergy = $"Energy: {TotalFood.Energy} / {TargetFood.Energy} kcal";
            SummaryFat = $"Fat: {TotalFood.Fat} / {TargetFood.Fat} gramm";
            SummaryCarbo = $"Carbo: {TotalFood.Carbo} / {TargetFood.Carbo} gramm";
            SummaryProtein = $"Protein: {TotalFood.Protein} / {TargetFood.Protein} gramm";
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
            foreach (var variationFoodItem in variationFoodItems)
            {
                var foodItem = variationFoodItem.FoodItem;
                foodItem.Quantity = variationFoodItem.Quantity;
                Items.Add(foodItem);
            }
            CalcSummary();
            IsBusy = false;
        }
        
        async Task ExecuteVariationItem_EditCommand()
        {
            await Navigation.PushAsync(new VariationItemPage(SelectedItem, MealVariation));
        }
        async Task ExecuteVariationItem_NewCommand()
        {
            await Navigation.PushAsync(new VariationItemPage(null, MealVariation));
        }

        

    }
}