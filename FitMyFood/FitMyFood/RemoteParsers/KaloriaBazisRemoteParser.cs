using System;
using System.Collections.Generic;

using FitMyFood.Models;
using DalSoft.RestClient;

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using System.Collections;

// https://restclient.dalsoft.io/docs
// https://github.com/DalSoft/DalSoft.RestClient/blob/master/DalSoft.RestClient.Test.Integration/RestClientTests.cs
namespace FitMyFood.RemoteParsers
{
    
    class KaloriaBazisRemoteParser : IRemoteParser
    {
        public FoodItem GetFoodItem(string name)
        {
            throw new NotImplementedException();
        }

        public void GetIcon()
        {
            throw new NotImplementedException();
        }

        public List<FoodItem> GetMatches(string pattern)
        {
            List<FoodItem> retnams = new List<FoodItem>();
            // https://kaloriabazis.hu/getfood.php?fav=true&q=r%C3%A9pa&p=1&s=8&expropsearch_id=0&expropsearch_inc=0&all_public_food=0
            dynamic client = new RestClient("https://kaloriabazis.hu/getfood.php");
            
            var result = client.Query(new {
                fav = true,
                q=pattern,
                p=1,
                s=8,
                expropsearch_id = 0,
                expropsearch_inc = 0,
                all_public_food = 0
                }).Get().Result;
            return retnams;
        }

       

    }
}
