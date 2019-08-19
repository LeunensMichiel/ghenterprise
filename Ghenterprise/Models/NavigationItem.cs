using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class NavigationItem : ObservableObject
    {
        public NavigationItem(string key)
        {
            Key = key;
        }

        public NavigationItem(int iconHex, string label, string key) : this(key)
        {
            Label = label;
            Icon = Char.ConvertFromUtf32(iconHex).ToString();
        }

        public string Icon { get; set; }
        public string Label { get; set; }
        public readonly string Key;


    }
}
