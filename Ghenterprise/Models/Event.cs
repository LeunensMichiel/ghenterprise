using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Event
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start_Date { get; set; }

        public DateTime End_Date { get; set; }

        public Enterprise Enterprise { get; set; }

    }
}
