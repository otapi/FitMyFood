using System.IO;
using Xamarin.Forms;
using FitMyFood.UWP;
using Windows.Storage;

[assembly: Dependency(typeof(FileHelper))]
namespace FitMyFood.UWP
{
    public class FileHelper : FitMyFood.Data.IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
