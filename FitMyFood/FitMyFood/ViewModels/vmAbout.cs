using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace FitMyFood.ViewModels
{
    public class VMAbout : VMBase
    {
        public VMAbout()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}