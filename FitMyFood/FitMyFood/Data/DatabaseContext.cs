using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using FitMyFood.Models;
namespace FitMyFood.Data
{
    public class DatabaseContext : DbContext
    {
        // TODO: OnModelCreating?
        // https://blog.oneunicorn.com/2017/09/25/many-to-many-relationships-in-ef-core-2-0-part-1-the-basics/
        public DbSet<ComposedFoodItem> ComposedFoodItems { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<DailyProfile> DailyProfiles { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Variation> Variations { get; set; }
        public DbSet<VariationFoodItem> VariationFoodItems { get; set; }
        public DbSet<WeightTrack> WeightTracks { get; set; }
        public DbSet<Settings> Settings { get; set; }

        private const string databaseName = "database1.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                    break;
                case Device.UWP:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
            App.PrintNote($"Path to database is: {databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* commented out to able to change the foodItem on it
            modelBuilder.Entity<VariationFoodItem>()
                .HasKey(t => new { t.FoodItemId, t.VariationId });
                */
        }
    }
}