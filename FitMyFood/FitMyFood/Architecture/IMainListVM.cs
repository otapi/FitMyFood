using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FitMyFood.Models;
using Xamarin.Forms;
namespace FitMyFood.Architecture
{
    interface MainListVMI
    {
        Command VariationItem_EditCommand { get; set; }
        Command VariationItem_NewCommand { get; set; }
    }
}
