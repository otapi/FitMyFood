using System;
using System.Collections.Generic;
using System.ComponentModel;


using Xamarin.Forms;

using FitMyFood.Models;
using FitMyFood.Data;

namespace FitMyFood.ViewModels
{
    public class BaseViewModel : Common.ObservableBase
    {
        public DataStore dataStore => new DataStore();

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
    }
}
