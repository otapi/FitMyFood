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
                Text = "Item 1",
                Description = "This is an item description."
            };
            init(item);
        }
        public ItemDetailViewModel(FoodItem item = null)
        {
            init(item);
        }

        void init(FoodItem item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
