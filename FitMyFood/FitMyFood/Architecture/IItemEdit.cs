using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FitMyFood.Architecture
{
    interface IItemEdit
    {
        Command SaveCommand { get; set; }
        Command CancelCommand { get; set; }
    }
}
