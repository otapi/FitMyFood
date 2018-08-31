using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FitMyFood.Common.Commands
{
    public class SaveFoodItem : ICommand
    {
        private bool _isBusy = false;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            SaveFoodItemAsync(parameter as Models.FoodItem);
        }

        private async void SaveFoodItemAsync(Models.FoodItem foodItem)
        {
            this._isBusy = true;
            this.RaiseCanExecuteChanged();
            
            await App.MainListFoodItemViewModel.dataStore.foodItems.UpdateItemAsync(foodItem);

            this._isBusy = false;
            this.RaiseCanExecuteChanged();
        }
    }
}
