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
        public Command LoadItemsCommand { get; set; }
        public Command SaveFoodItemCommand { get; set; }

        public VMMainListFoodItem()
        {
            Title = "Browse";
            Items = new ObservableCollection<FoodItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveFoodItemCommand = new Command<FoodItem>(async (foodItem) => await ExecuteSaveFoodItemCommand(foodItem));
            
            MessagingCenter.Subscribe<NewItemPage, FoodItem>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as FoodItem;
                Items.Add(newItem);
                await dataStore.foodItems.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await dataStore.foodItems.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task ExecuteSaveFoodItemCommand(FoodItem foodItem)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await App.vmMainListFoodItem.dataStore.foodItems.UpdateItemAsync(foodItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}