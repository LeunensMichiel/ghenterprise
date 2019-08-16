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
        private int _id;
        private String _firstname;
        private String _lastname;
        private String _email;
        private String _password;

        public User()
        {
            _id = 0;
            _firstname = "";
            _lastname = "";
            _email = "";
            _password = "";
        }

        public int id {
            get
            {
                return _id;
            }
            set
            {
                Set("id", ref _id, value);
            }
        }
        public String firstname {
            get
            {
                return _firstname;
            }
            set
            {
                Debug.WriteLine("USER_FIRSTNAME_CHECK");
                Set("firstname", ref _firstname, value);
            }
        }
        public String lastname {
            get
            {
                return _lastname;
            }
            set
            {
                Set("lastname", ref _lastname, value);
            }
        }
        public String email {
            get
            {
                return _email;
            }
            set
            {
                Set("email", ref _email, value);
            }
        }
        public String password {
            get
            {
                return _password;
            }
            set
            {
                Set("password", ref _password, value);
            }
        }

    }
}
