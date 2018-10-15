using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FitMyFood.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FitMyFood
{
    public partial class App : Application
    {
        public static ViewModels.MainListVM MainListVM { get; set; }
        public static ViewModels.FoodItemVM FoodItemVM { get; set; }
        public static ViewModels.VariationItemVM VariationItemVM { get; set; }
        public static ViewModels.MenuVM MenuVM { get; set; }
        
        static Data.Database _DB;
        public static Data.Database DB
        {
            get
            {
                if (_DB == null)
                {
                    App.PrintNote($"[DatabaseHelper] start");
                    
                    _DB = Data.Database.Create();
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
            App.PrintNote($"[{this.GetType().Name}/{System.Reflection.MethodBase.GetCurrentMethod().Name}] ping");
            MenuVM = new ViewModels.MenuVM(null);

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
