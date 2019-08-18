using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    class Response
    {
        private string _message;

        public string message {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
    }
}
