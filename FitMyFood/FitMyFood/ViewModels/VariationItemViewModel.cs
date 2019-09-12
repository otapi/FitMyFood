using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using FitMyFood.Services.RemoteParsers;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers;
using System.Linq;

namespace FitMyFood.ViewModels
{
    public class VariationItemViewModel : BaseViewModel
    {
        public IAsyncCommand FoodItem_EditCommand { get; set; }
        public IAsyncCommand MainList_RemoveItemCommand { get; set; }
        public IAsyncCommand FoodItem_NewCommand { get; set; }
        public IAsyncCommand MainList_EditFinishedCommand { get; set; }
        public IAsyncCommand<string> FillSearchFoodItemsCommand { get; set; }
        public IAsyncCommand ChangeQuantityCommand { get; set; }
        
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
        VariationItemSearchItem _SelectedSearchItem;
        public VariationItemSearchItem SelectedSearchItem
        {
            get { return _SelectedSearchItem; }
            set
            {
                SetProperty(ref _SelectedSearchItem, value);
                if (value != null)
                {
                    FoodItem selectedFoodItem = value.GetFoodItem();
                    if (VariationFoodItem == null)
                    {
                        if (Item == null)
                        {
                            VariationFoodItem = App.DB.AddNewVariationFoodItemAsync(selectedFoodItem.Quantity, Variation, selectedFoodItem).Result;
                            App.DB.SaveChangesNoWait();
                        } else {
                            VariationFoodItem = App.DB.GetVariationFoodItemAsync(selectedFoodItem, Variation).Result;
                        }
                        
                    }
                    if (!App.DB.IsExistFoodItem(selectedFoodItem))
                    {
                        App.DB.AddFoodItem(selectedFoodItem).Wait();
                    }
                    Item = App.DB.GetFoodItemAsNoTracked(selectedFoodItem).Result;
                    VariationFoodItem.FoodItem = selectedFoodItem;
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
        public ObservableRangeCollection<VariationItemSearchItem> SearchItems { get; set; }
        
        public Variation Variation;
        public VariationFoodItem VariationFoodItem;

        public double OrigEnergy;

        KaloriaBazisRemoteParser kaloriaBazisRemoteParser;


        public VariationItemViewModel(FoodItem foodItem, Variation variation)
        {
            FoodItem_EditCommand = new AsyncCommand(FoodItem_Edit);
            MainList_RemoveItemCommand = new AsyncCommand(MainList_RemoveItem);
            FoodItem_NewCommand = new AsyncCommand(FoodItem_New);
            MainList_EditFinishedCommand = new AsyncCommand(MainList_EditFinished);
            FillSearchFoodItemsCommand = new AsyncCommand<string>(FillSearchFoodItems);
            ChangeQuantityCommand = new AsyncCommand(ChangeQuantity);

            Item = foodItem;
            OrigEnergy = (foodItem == null ? 0 : foodItem.Energy);
            SearchItems = new ObservableRangeCollection<VariationItemSearchItem>();
            IsSearchItemsListviewVisible = false;

            Variation = variation;
        }

        async Task FoodItem_Edit()
        {
            IsBusy = true;
            await App.Navigation.PushAsync(new FoodItemPage(await App.DB.GetFoodItemAsTracked(Item), null));
            IsBusy = false;
        }

        async Task FoodItem_New()
        {
            IsBusy = true;
            await App.Navigation.PushAsync(new FoodItemPage(null, Variation));
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
            VariationFoodItem = await App.DB.GetVariationFoodItemAsync(Item, Variation);
            App.DB.Remove(VariationFoodItem);
            await App.DB.SaveChangesAsync();
            IsBusy = false;
            await App.Navigation.PopAsync(true);
        }
        async Task MainList_EditFinished()
        {
            await App.Navigation.PopAsync(true);
        }

        async Task FillSearchFoodItems(string term)
        {
            IsBusy = true;
            App.VariationItemViewModel.IsSearchItemsListviewVisible = true;
            SearchItems.Clear();
            SearchItems.AddRange(
                from item in await App.DB.GetOrderedFoodItemsAsync(term)
                select new VariationItemSearchItem()
                {
                    Source = null,
                    Name = item.Name,
                    Icon = null,
                    InternalFoodItem = item
                }
            );

            // External sources
            if (kaloriaBazisRemoteParser == null)
            {
                kaloriaBazisRemoteParser = new KaloriaBazisRemoteParser();
            }
            await FillSearchItems_ExternalSources(kaloriaBazisRemoteParser, term);
            IsBusy = false;
        }

        async Task FillSearchItems_ExternalSources(IRemoteParser source, string term)
        {
            SearchItems.AddRange(
                from item in await source.GetMatches(term)
                where !(from s in SearchItems
                       select s.Name).Contains(item)
                select new VariationItemSearchItem()
                {
                    Name = item,
                    Icon = source.GetIcon(),
                    Source = source,
                }
            );
        }

        private void SuggestWeight()
        {
            if (Item.Quantity == 0)
            {
                Item.Quantity = 1;
            }
            double energyInOneGramm = Item.Energy / (Item.Weight);
            double missingEnergy = App.MainListViewModel.TargetFood.Energy -
                    (App.MainListViewModel.TotalFood.Energy - OrigEnergy);
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
