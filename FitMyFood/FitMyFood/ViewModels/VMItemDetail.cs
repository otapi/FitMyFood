using System;

using FitMyFood.Models;

namespace FitMyFood.ViewModels
{
    public class VMItemDetail : VMBase
    {
        public FoodItem Item { get; set; }
        public VMItemDetail()
        {
            var item = new FoodItem
            {
                Name = "Item 1",
                UnitDescription = "This is an item description."
            };
            init(item);
        }
        public VMItemDetail(FoodItem item = null)
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
