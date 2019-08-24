using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Street:ObservableObject
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
