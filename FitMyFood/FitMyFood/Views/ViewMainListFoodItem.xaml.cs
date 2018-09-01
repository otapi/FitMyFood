using FitMyFood.Models;
using FitMyFood.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewMainListFoodItem : ContentView
	{
		public ViewMainListFoodItem ()
		{
			InitializeComponent ();
            if (App.vmMainListFoodItem.Items.Count == 0)
                App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as FoodItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new VMItemDetail(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        void OnStepperChanged(object sender, EventArgs e)
        {
            FoodItem foodItem = (sender as Stepper).BindingContext as FoodItem;
            App.vmMainListFoodItem.SaveFoodItemCommand.Execute(foodItem);
        }
    }
}