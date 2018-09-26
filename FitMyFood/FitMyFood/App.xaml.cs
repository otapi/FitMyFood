using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitMyFood.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FitMyFood
{
    public partial class App : Application
    {
        public static ViewModels.MainListFoodItemVM MainListFoodItemVM { get; set; }
        public static ViewModels.ItemEditVM ItemEditVM { get; set; }
        public static ViewModels.ItemViewVM ItemViewVM { get; set; }
        public static Data.DatabaseHelper DB { get; set; }
        
        public static void PrintWarning(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[FitMyFood][Warning] {message}");
        }
        public static void PrintNote(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[FitMyFood][Note] {message}");
        }
        
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
