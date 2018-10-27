using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.RemoteParsers
{
    public interface IRemoteParser
    {
        void GetIcon();
        List<Models.FoodItem> GetMatches(string pattern);
        FitMyFood.Models.FoodItem GetFoodItem(string name);
    }
}
