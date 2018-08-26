using SQLite;

namespace FitMyFood.Models
{
    public class FoodItem : DataStoreItemMaster
    {
        public string Text { get; set; }
        public string Description { get; set; }
    }
}