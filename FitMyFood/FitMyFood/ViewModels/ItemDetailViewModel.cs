using System;

using FitMyFood.Models;

namespace FitMyFood.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public FoodItem Item { get; set; }
        public ItemDetailViewModel()
        {
            var item = new FoodItem
            {
                Name = "Item 1",
                UnitDescription = "This is an item description."
            };
            init(item);
        }
        public ItemDetailViewModel(FoodItem item = null)
        {
            init(item);
        }

        void init(FoodItem item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }
}
