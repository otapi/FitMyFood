using System;
using System.Windows.Input;

using Xamarin.Forms;
using MvvmHelpers.Interfaces;
using MvvmHelpers.Commands;

namespace FitMyFood.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public IAsyncCommand OpenWebCommand { get; }
        public AboutViewModel(INavigation navigation) : base(navigation)
        {
            Title = "About";

            OpenWebCommand = new AsyncCommand(async () => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        
    }
}