using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;
using System.Collections.ObjectModel;

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

        public ObservableCollection<WeightTrack> Weights { get; set; }
        public WeightTrackViewModel(INavigation navigation) : base(navigation)
        {
            
            Weights = new ObservableCollection<WeightTrack>();
            var t = LoadWeights();
            t.Wait();
            ActualWeight = App.MainListViewModel.Settings.ActualWeight;
        }

        async Task LoadWeights()
        {
            Weights.Clear();

            foreach (var w in await App.DB.GetWeightTracks())
            {
                Weights.Add(w);
            }
            IsBusy = false;
        }
    }
}
