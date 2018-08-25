using System;

using FitMyFood.Models;

namespace FitMyFood.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel()
        {
            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };
            init(item);
        }
        public ItemDetailViewModel(Item item = null)
        {
            init(item);
        }

        void init(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
