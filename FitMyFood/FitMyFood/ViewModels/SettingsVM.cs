using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

namespace FitMyFood.ViewModels
{
    public class SettingsVM : BaseVM
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
                SetProperty(ref _Settings, value);
                if (!internalChange)
                {
                    internalChange = true;
                    SexPicker = (value.Sex ? 1 : 0);
                    ActivityPicker = value.Physical_activity - 1;
                    internalChange = false;
                }
                App.DB.SaveChangesNoWait();
                App.MainListVM.Settings = Settings;
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
                if (!internalChange)
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
                if (!internalChange)
                {
                    internalChange = true;
                    Settings.Physical_activity = value + 1;
                    internalChange = false;
                }
            }
        }

        public SettingsVM(INavigation navigation) : base(navigation)
        {
            
            //Title = "Settings";
            var t = App.DB.GetSettings();
            t.Wait();
            Settings = t.Result;
        }

    }
}
