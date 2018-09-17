using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FitMyFood.Models;
using FitMyFood.Views;
using FitMyFood.ViewModels;
using System.Collections.ObjectModel;
using FitMyFood.CustomControls;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainListFoodItemPage : ContentPage, ISwipeCallBack
    {
        public MainListFoodItemPage()
        {
            InitializeComponent();

            BindingContext = App.vmMainListFoodItem;
            App.vmMainListFoodItem.navigation = Navigation;
            App.vmMainListFoodItem.LoadSelectorsCommand.Execute(null);

            SwipeListener swipeListener = new SwipeListener(lbl_swipe, this);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            /*
            var item = args.SelectedItem as FoodItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new VMItemDetail(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
            */
        }

        void OnStepperChanged(object sender, EventArgs e)
        {
            FoodItem foodItem = (sender as Stepper).BindingContext as FoodItem;
            if (foodItem != null)
            {
                App.vmMainListFoodItem.SaveFoodItemCommand.Execute(foodItem);
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ItemEditPage(new VMItemDetail())));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        void OnButtonClick(object sender, EventArgs e)
        {
            var it = App.vmMainListFoodItem;
            App.vmMainListFoodItem.LoadItemsCommand.Execute(null);
        }

        public void OnEdit(object sender, EventArgs e)
        {
            ViewCell aa = new ViewCell();
            
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }

        public void onBottomSwipe(View view)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "OnBottomSwipe";
            }
        }

        public void onLeftSwipe(View view)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "onLeftSwipe";
            }
        }

        public void onNothingSwiped(View view)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "onNothingSwiped";
            }
        }

        public void onRightSwipe(View view)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "onRightSwipe";
            }
        }

        public void onTopSwipe(View view)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "onTopSwipe";
            }
        }

        public void onLeftSwipeProcess(View view, double delta)
        {
            if (view == lbl_swipe)
            {
                lbl_result.Text = "onLeftSwipe running";
                view.BackgroundColor = Color.CadetBlue;
                view.TranslateTo(delta, 0);
            }
        }
    }
}