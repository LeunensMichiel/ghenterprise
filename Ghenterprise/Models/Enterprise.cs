using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Enterprise:ObservableObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime Date_Created { get; set; }

        public Location Location { get; set; }

        public List<Event> Events { get; set; }

        public List<Promotion> Promotions { get; set; }

        public List<Category> Categories { get; set; }

        public List<Tag> Tags { get; set; }

    }
}
