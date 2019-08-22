using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class File
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public Enterprise Enterprise { get; set; }

        public User User { get; set; }
    }
}
