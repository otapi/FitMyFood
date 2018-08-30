using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class MainListFoodItemViewModel : BaseViewModel
    {
        ObservableCollection<FoodItem> items;
        public ObservableCollection<FoodItem> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
            }
        }
        public Command LoadItemsCommand { get; set; }

        public MainListFoodItemViewModel()
        {
            Title = "Browse";
            items = new ObservableCollection<FoodItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

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
    }
}