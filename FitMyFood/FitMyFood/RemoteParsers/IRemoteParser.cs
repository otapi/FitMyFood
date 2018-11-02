using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMyFood.RemoteParsers
{
    public interface IRemoteParser
    {
        void GetIcon();
        Task<List<Models.FoodItem>> GetMatches(string pattern);
        FitMyFood.Models.FoodItem GetFoodItem(string name);
    }
}
