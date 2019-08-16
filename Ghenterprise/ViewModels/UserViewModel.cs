using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.ViewModels
{
    public class UserViewModel:ViewModelBase
    {
        private User _user = new User();

        private RelayCommand _registerCommand;

        private UserService _userService = new UserService();

        public User User {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }


        public String Firstname {
            get
            {
                return _user.firstname;
            }
            set
            {
                Debug.WriteLine("FIRSTNAME_CHANGE");
                _user.firstname = value;
            }
        }

        public RelayCommand RegisterCommand {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(async () =>
                    {
                        Debug.WriteLine("REGISTER_COMMAND");
                        Debug.WriteLine(JsonConvert.SerializeObject(User).GetType());
                        var req = await _userService.RegisterUser(User);
                    });
                }

                return _registerCommand;
            }
        }
    }
}
