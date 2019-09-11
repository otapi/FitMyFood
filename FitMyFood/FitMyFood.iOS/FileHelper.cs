using System;
using System.IO;
using Xamarin.Forms;
using FitMyFood.iOS;

[assembly: Dependency(typeof(FileHelper))]
namespace FitMyFood.iOS
{
    public class FileHelper : FitMyFood.Services.IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}
