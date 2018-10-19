using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace FitMyFood.ViewModels
{
    public class VariationItemVM : BaseVM
    {
        public Command FoodItem_EditCommand { get; set; }
        public Command MainList_RemoveItemCommand { get; set; }
        public Command FoodItem_NewCommand { get; set; }
        public Command MainList_EditFinishedCommand { get; set; }
        public Command FillSearchFoodItemsCommand { get; set; }
        public Command ChangeQuantityCommand { get; set; }
        
        FoodItem _Item;
        public FoodItem Item
        {
            get { return _Item; }
            set
            {
                IsDetailsVisible = value != null;
                if (value != null)
                {
                    if (_Item == null || value.Weight != _Item.Weight)
                    {
                        _Weight = value.Weight;
                    }
                }
                SetProperty(ref _Item, value);
            }
        }
        bool _IsSearchItemsListviewVisible;
        public bool IsSearchItemsListviewVisible
        {
            get { return _IsSearchItemsListviewVisible; }
            set
            {
                SetProperty(ref _IsSearchItemsListviewVisible, value);
            }
        }
        bool _IsDetailsVisible;
        public bool IsDetailsVisible
        {
            get { return _IsDetailsVisible; }
            set
            {
                SetProperty(ref _IsDetailsVisible, value);
            }
        }
        FoodItem _SelectedSearchItem;
        public FoodItem SelectedSearchItem
        {
            get { return _SelectedSearchItem; }
            set
            {
                SetProperty(ref _SelectedSearchItem, value);
                if (value != null)
                {

                    if (VariationFoodItem == null)
                    {
                        if (Item == null)
                        {
                            VariationFoodItem = App.DB.AddNewVariationFoodItemAsync(value.Quantity, Variation, value).Result;
                            App.DB.SaveChangesNoWait();
                        } else {
                            VariationFoodItem = App.DB.GetVariationFoodItemAsync(value, Variation).Result;
                        }
                        
                    }
                    Item = App.DB.GetFoodItemAsNoTracked(value).Result;
                    VariationFoodItem.FoodItem = value;
                    VariationFoodItem.Quantity = Item.Quantity;
                    App.DB.SaveChangesNoWait();
                    IsSearchItemsListviewVisible = false;
                    SuggestWeight();
                    SelectedSearchItem = null;
                }
            }
        }

        double _Weight;
        public double Weight
        {
            get { return _Weight; }
            set
            {
                if (value != _Weight)
                {
                    if (Item != null)
                    {
                        var newQuantity = value / Item.UnitWeight;
                        if (newQuantity != Item.Quantity)
                        {
                            _Item.Quantity = newQuantity;
                            OnPropertyChanged("Item");
                            ChangeQuantityCommand.Execute(null);
                        }
                    }
                    SetProperty(ref _Weight, value);
                }
            }
        }
        public ObservableCollection<FoodItem> SearchItems { get; set; }
        
        public Variation Variation;
        public VariationFoodItem VariationFoodItem;

        public double OrigEnergy;

        public VariationItemVM(INavigation navigation, FoodItem foodItem, Variation variation) : base(navigation)
        {
            FoodItem_EditCommand = new Command(async () => await FoodItem_Edit());
            MainList_RemoveItemCommand = new Command(async () => await MainList_RemoveItem());
            FoodItem_NewCommand = new Command(async () => await FoodItem_New());
            MainList_EditFinishedCommand = new Command(async () => await MainList_EditFinished());
            FillSearchFoodItemsCommand = new Command<string>(async (string term) => await FillSearchFoodItems(term));
            ChangeQuantityCommand = new Command(async () => await ChangeQuantity());

            Item = foodItem;
            OrigEnergy = (foodItem == null ? 0 : foodItem.Energy);
            SearchItems = new ObservableCollection<FoodItem>();
            IsSearchItemsListviewVisible = false;

            Variation = variation;
        }

        async Task FoodItem_Edit()
        {
            IsBusy = true;
            await Navigation.PushAsync(new FoodItemPage(await App.DB.GetFoodItemAsTracked(Item), null));
            IsBusy = false;
        }

        async Task FoodItem_New()
        {
            IsBusy = true;
            await Navigation.PushAsync(new FoodItemPage(null, Variation));
            IsBusy = false;
        }

        public async Task ChangeQuantity()
        {
            if (Item == null)
            {
                return;
            }
            if (VariationFoodItem == null)
            {
                VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            }
            VariationFoodItem.Quantity = Item.Quantity;
            await App.DB.SaveChangesAsync();
            Weight = Item.Weight;
        }

        async Task MainList_RemoveItem()
        {
            if (Item == null)
            {
                return;
            }
            if (VariationFoodItem == null)
            {
                VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            }
            VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            App.DB.Remove(VariationFoodItem);
            await App.DB.SaveChangesAsync();
            IsBusy = false;
            await Navigation.PopAsync(true);
        }
        async Task MainList_EditFinished()
        {
            await Navigation.PopAsync(true);
        }

        async Task FillSearchFoodItems(string term)
        {
            App.VariationItemVM.IsSearchItemsListviewVisible = true;
            SearchItems.Clear();
            foreach (var item in await App.DB.GetOrderedFoodItemsAsync(term))
            {
                SearchItems.Add(item);
            };
        }

        private void SuggestWeight()
        {
            if (Item.Quantity == 0)
            {
                Item.Quantity = 1;
            }
            double energyInOneGramm = Item.Energy / (Item.Weight);
            double missingEnergy = App.MainListVM.TargetFood.Energy -
                    (App.MainListVM.TotalFood.Energy - OrigEnergy);
            if (missingEnergy > 0 && energyInOneGramm != 0)
            {
                Weight = (int)(missingEnergy / energyInOneGramm);
            }
            else
            {
                // TODO: warning to exceed the energy limit
                Weight = 100;
            }
        }

    }
}
