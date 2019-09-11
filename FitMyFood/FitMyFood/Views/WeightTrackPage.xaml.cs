using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FitMyFood.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeightTrackPage : ContentPage
    {
        public WeightTrackPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.WeightTrackViewModel();
        }
    }
}