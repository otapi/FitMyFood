using System;
using System.Collections.Generic;
using System.Text;

namespace FitMyFood.Models
{
    public enum MenuItemType
    {
        MainList,
        Settings,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
