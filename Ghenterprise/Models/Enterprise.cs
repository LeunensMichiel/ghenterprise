using GalaSoft.MvvmLight;
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

        public DateTime DateCreated { get; set; }

        public Location Location { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Promotion> Promotions { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Tag> Tags { get; set; }

    }
}
