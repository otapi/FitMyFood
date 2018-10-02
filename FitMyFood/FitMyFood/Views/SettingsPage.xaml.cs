using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.SettingsVM(Navigation);
        }
    }
}