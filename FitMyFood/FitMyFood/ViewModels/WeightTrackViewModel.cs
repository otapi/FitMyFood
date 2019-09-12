using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using System.Collections.ObjectModel;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;
using MvvmHelpers;
// TODO: OxyPlot.Xamarin.Forms - http://docs.oxyplot.org/en/latest/getting-started/hello-xamarin-forms.html

namespace FitMyFood.ViewModels
{
    public class WeightTrackViewModel : BaseViewModel
    {
        double _ActualWeight;
        public double ActualWeight
        {
            get
            {
                
                return _ActualWeight;
            }
            set
            {
                SetProperty(ref _ActualWeight, value);
                var t = App.DB.SetWeightTrack(new WeightTrack()
                {
                    Date = DateTime.Today,
                    Weight = value
                });
                t.Wait();
                App.MainListViewModel.Settings.ActualWeight = value;
                App.DB.SaveChangesNoWait();
            }
        }

        public ObservableRangeCollection<WeightTrack> Weights { get; set; }
        public WeightTrackViewModel()
        {
            
            Weights = new ObservableRangeCollection<WeightTrack>();
            var t = LoadWeights();
            t.Wait();
            ActualWeight = App.MainListViewModel.Settings.ActualWeight;
        }

        async Task LoadWeights()
        {
            IsBusy = true;
            Weights.Clear();
            Weights.AddRange(await App.DB.GetWeightTracks());
            IsBusy = false;
        }
    }
}
