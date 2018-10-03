using System;

using FitMyFood.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using FitMyFood.Views;

// TODO: navigation with back button, and no need for save and cancel
// TODO: convert Physical_activity to a picker
// TODO: convert sex to boolean picer
// TODO: scroll the page

namespace FitMyFood.ViewModels
{
    public class SettingsVM : BaseVM
    {
        
        Settings _Settings;
        public Settings Settings {
            get
            {
                
                return _Settings;
            }
            set
            {
                SetProperty(ref _Settings, value);
                App.DB.SaveChangesNoWait();
                App.MainListVM.Settings = Settings;
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
