using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        bool internalChange = false;
        Settings _Settings;
        public Settings Settings {
            get
            {
                
                return _Settings;
            }
            set
            {
                if (_Settings != null && value != null && _Settings.ActualWeight != value.ActualWeight)
                {
                    Task.Run(async () =>
                    {
                        await App.DB.SetWeightTrack(new WeightTrack()
                        {
                            Date = DateTime.Today,
                            Weight = value.ActualWeight
                        });
                        await App.DB.SaveChangesAsync();
                    });
                }
                SetProperty(ref _Settings, value);
                if (!internalChange)
                {
                    internalChange = true;
                    SexPicker = (value.Sex ? 1 : 0);
                    ActivityPicker = value.Physical_activity - 1;
                    internalChange = false;
                }
                
                App.MainListViewModel.Settings = Settings;
            }
        }
        int _SexPicker;
        public int SexPicker
        {
            get
            {
                
                return _SexPicker;
            }
            set
            {
                SetProperty(ref _SexPicker, value);
                if (!internalChange && Settings != null)
                {
                    internalChange = true;
                    Settings.Sex = value == 1;
                    internalChange = false;
                }
            }
        }
         int _ActivityPicker;
        public int ActivityPicker
        {
            get
            {
                
                return _ActivityPicker;
            }
            set
            {
                SetProperty(ref _ActivityPicker, value);
                if (!internalChange && Settings != null)
                {
                    internalChange = true;
                    Settings.Physical_activity = value + 1;
                    internalChange = false;
                }
            }
        }

        public SettingsViewModel(INavigation navigation) : base(navigation)
        {

            //Title = "Settings";
            Task.Run(async () =>
            {
                Settings = await App.DB.GetSettings();
            });
        }

    }
}
