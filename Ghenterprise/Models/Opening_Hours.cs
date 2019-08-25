using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    class Opening_Hours
    {
        public string Id { get; set; }
        public int Day_Of_Week { get; set; }
        public string Morning_Start { get; set; }
        public string Morning_End { get; set; }
        public string Afternoon_Start { get; set; }
        public string Afternoon_End { get; set; }
    }
}
