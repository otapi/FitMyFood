using System;
using System.IO;
using Xamarin.Forms;
using FitMyFood.Droid;

[assembly: Dependency(typeof(FileHelper))]
namespace FitMyFood.Droid
{
    public class FileHelper : FitMyFood.Data.IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
