using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Notification
    {
        public string Id { get; set; }

        public Enterprise Enterprise { get; set; }

        public Promotion Promotion { get; set; }

        public Event Event { get; set; }
    }
}
