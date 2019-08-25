using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class User:ObservableObject
    {
        public string Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public ICollection<Enterprise> Enterprises { get; set; }

        public ICollection<Enterprise> Subscriptions { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
