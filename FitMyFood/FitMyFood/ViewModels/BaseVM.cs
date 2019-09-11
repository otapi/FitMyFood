using System;
using System.Collections.Generic;
using System.ComponentModel;


using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Services;
using System.Runtime.CompilerServices;

namespace FitMyFood.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public INavigation Navigation { get; set; }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            //if (EqualityComparer<T>.Default.Equals(backingStore, value))
            //    return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public BaseVM(INavigation navigation)
        {
            Navigation = navigation;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
