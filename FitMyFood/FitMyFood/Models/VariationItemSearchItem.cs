using FitMyFood.RemoteParsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{

    public class VariationItemSearchItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public IRemoteParser Source { get; set; }
        public FoodItem InternalFoodItem { get; set; }
        public FoodItem GetFoodItem()
        {
            if (Source == null)
            {
                return InternalFoodItem;
            }
            else
            {
                return Source.GetFoodItem(Name);
            }
        }
    }
}
