using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMyFood.RemoteParsers
{
    public interface IRemoteParser
    {
        string GetIcon();
        string GetSource();
        Task<List<string>> GetMatches(string pattern);
        Models.FoodItem GetFoodItem(string name);
    }
}
