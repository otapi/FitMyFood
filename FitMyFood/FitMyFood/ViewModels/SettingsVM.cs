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
        public Command MainList_SaveCommand { get; set; }
        public Command MainList_CancelCommand { get; set; }
        Settings _Settings;
        public Settings Settings {
            get
            {
                
                return _Settings;
            }
            set
            {
                SetProperty(ref _Settings, value);

            }
        }

        public SettingsVM(INavigation navigation) : base(navigation)
        {
            MainList_SaveCommand = new Command(async () => await MainList_Save());
            MainList_CancelCommand = new Command(async () => await MainList_Cancel());

            //Title = "Settings";
            var t = App.DB.GetSettings();
            t.Wait();
            Settings = t.Result;
        }

        async Task MainList_Save()
        {
            IsBusy = true;
            await App.DB.SaveChangesAsync();
            App.MainListVM.Settings = Settings;
            IsBusy = false;
            await Navigation.PopAsync(true);
        }
        async Task MainList_Cancel()
        {
            await Navigation.PopAsync(true);
        }
    }
}
