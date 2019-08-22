using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Location
    {
        public string Id { get; set; }

        public Street Street { get; set; }

        public City City { get; set; }

        public int Number { get; set; }
    }
}
