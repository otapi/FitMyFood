using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.AboutViewModel(Navigation);
        }
    }
}