using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    class Enterprise:ObservableObject
    {
        private string _id;
        private string _name;
        private string _description;
        private DateTime _datecreated;

        public Enterprise()
        {
            _id = "";
            _name = "";
            _description = "";
            _datecreated = new DateTime();
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                Set("Id", ref _id, value);
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set("Name", ref _name, value);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                Set("Description", ref _description, value);
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return _datecreated;
            }
            set
            {
                Set("DateCreated", ref _datecreated, value);
            }
        }
    }
}
