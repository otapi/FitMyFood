﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Data
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
