using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitMyFood.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FitMyFood
{
    public partial class App : Application
    {
        // Global ViewModels
        public static ViewModels.MainListViewModel MainListViewModel { get; set; }
        public static ViewModels.FoodItemViewModel FoodItemViewModel { get; set; }
        public static ViewModels.VariationItemViewModel VariationItemViewModel { get; set; }
        public static ViewModels.MenuViewModel MenuViewModel { get; set; }

        // Global Services
        public static INavigation Navigation { get; set; }

        static Services.Database _DB;
        public static Services.Database DB
        {
            get
            {
                if (_DB == null)
                {
                    App.PrintNote($"[DatabaseHelper] start");
                    
                    _DB = Services.Database.Create();
                    App.PrintNote($"[DatabaseHelper] end");

                }
                return _DB;
            }
        }

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
#if DEBUG
            App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] ping");
#endif
            MenuViewModel = new ViewModels.MenuViewModel();

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
