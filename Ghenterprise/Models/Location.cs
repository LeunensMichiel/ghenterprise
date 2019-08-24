using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Location:ObservableObject
    {
        public string Id { get; set; }

        public Street Street { get; set; }

        public City City { get; set; }

        public int Street_Number { get; set; }
        public Location()
        {
            Street = new Street();
            City = new City();
        }
    }
}
