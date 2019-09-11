using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Services
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
