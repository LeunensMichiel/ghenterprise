using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Opening_Hours
    {
        public string Id { get; set; }
        public int Day_Of_Week { get; set; }
        public DateTime Morning_Start { get; set; }
        public DateTime Morning_End { get; set; }
        public DateTime Afternoon_Start { get; set; }
        public DateTime Afternoon_End { get; set; }
    }
}
