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
// http://blog.masterdevs.com/xf-day-2/
// https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
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

        string normalize(string str)
        {
            return str.Replace("<b>", "").Replace("</b>", "");
        }
        public async Task<List<FoodItem>> GetMatches(string pattern)
        {
            List<FoodItem> retnams = new List<FoodItem>();
            if (pattern != null && pattern.Length > 2)
            {
                // https://kaloriabazis.hu/getfood.php?fav=true&q=r%C3%A9pa&p=1&s=8&expropsearch_id=0&expropsearch_inc=0&all_public_food=0
                dynamic client = new RestClient("https://kaloriabazis.hu");
                KalObject result = null;
                try
                {
                    result = await client.Resource("getfood.php").Query(new
                    {
                        fav = true,
                        q = pattern,
                        p = 1,
                        s = 8,
                        expropsearch_id = 0,
                        expropsearch_inc = 0,
                        all_public_food = 0,
                        
                    }).Get();
                } catch
                {
                    return retnams;
                }
                foreach (var f in result.results2)
                {
                    retnams.Add(new FoodItem()
                    {
                        Name = normalize(f.name),
                        Protein = double.Parse(f.protein),
                        Carbo = double.Parse(f.carbo),
                        Fat = double.Parse(f.fat),
                        UnitDescription = "gramm"

                    });
                }
            }
             
            return retnams;
        }

       

    }

 

    public class Results2
    {
        public string id { get; set; }
        public string food_id { get; set; }
        public string kuktalink { get; set; }
        public string kuktapic { get; set; }
        public string kuktatitle { get; set; }
        public string votedpic { get; set; }
        public string votedpictitle { get; set; }
        public string name { get; set; }
        public string cDesc { get; set; }
        public string pic { get; set; }
        public string pictitle { get; set; }
        public string pictrash { get; set; }
        public string pictrashtitle { get; set; }
        public string piece { get; set; }
        public string weight { get; set; }
        public string cal { get; set; }
        public string protein { get; set; }
        public string carbo { get; set; }
        public string fat { get; set; }
        public string link { get; set; }
        public string linkname { get; set; }
        public string link2 { get; set; }
        public string unvoteddelvotedpic { get; set; }
        public string unvoteddelvotedpictitle { get; set; }
        public string datasheetlink { get; set; }
    }

    public class UsdaLinksArr
    {
        public string __invalid_name__2315 { get; set; }
        public string __invalid_name__2317 { get; set; }
        public object __invalid_name__2335 { get; set; }
        public string __invalid_name__432777 { get; set; }
        public string __invalid_name__1502333 { get; set; }
    }

    public class KalObject
    {
        public int rownum1 { get; set; }
        public string total1 { get; set; }
        public List<object> results1 { get; set; }
        public int total2 { get; set; }
        public List<Results2> results2 { get; set; }
        public UsdaLinksArr usdaLinks_arr { get; set; }
        public List<object> rec_usda_empty_arr { get; set; }
    }
}
