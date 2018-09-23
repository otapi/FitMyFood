using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FitMyFood.ViewModels
{
    public class AboutVM : BaseVM
    {
        public AboutVM(INavigation navigation) : base(navigation)
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}