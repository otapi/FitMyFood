using FitMyFood.Services.RemoteParsers;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitMyFood.Models
{

    public class VariationItemSearchItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public IRemoteParser Source { get; set; }
        public FoodItem InternalFoodItem { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string NameSort => Name[0].ToString();

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
