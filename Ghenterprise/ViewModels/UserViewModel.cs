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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using static System.Net.WebRequestMethods;

namespace Ghenterprise.ViewModels
{
    public class UserViewModel:ViewModelBase
    {

        private UserService userService;

        private User _user = new User();
        private String _buttonText = "Registreren";
        private String _errorText = "";
        private Boolean _buttonEnabled = true;
        private Visibility _textBoxVisibility = Visibility.Visible;
        private Visibility _loginBtnVisibility = Visibility.Collapsed;
        private String _userSwitchLinkText = "Ik heb al een account";
        private String _loginBtnText = "Aanmelden";

        private RelayCommand _registerCommand;
        private RelayCommand _switchUserScreen;
        private RelayCommand _loginCommand;

        public UserViewModel()
        {
            userService = new UserService();
        }

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

        public String ButtonText {
            get
            {
                return _buttonText;
            }
            set
            {
                Set("ButtonText", ref _buttonText, value);
            }
        }

        public String ErrorText {
            get
            {
                return _errorText;
            }
            set
            {
                Set("ErrorText", ref _errorText, value);
            }
        }

        public Boolean ButtonEnabled {
            get
            {
                return _buttonEnabled;
            }
            set
            {
                Set("ButtonEnabled", ref _buttonEnabled, value);
            }
        }

        public Visibility TextBoxVisibility {
            get
            {
                return _textBoxVisibility;
            }
            set
            {
                Set("TextBoxVisibility", ref _textBoxVisibility, value);
            }
        }

        public Visibility LoginBtnVisibility {
            get
            {
                return _loginBtnVisibility;
            }
            set
            {
                Set("LoginBtnVisibility", ref _loginBtnVisibility, value);
            }
        }

        public String UserSwitchLinkText {
            get
            {
                return _userSwitchLinkText;
            }
            set
            {
                Set("UserSwitchLinkTest", ref _userSwitchLinkText, value);
            }
        }

        public String LoginBtnText
        {
            get
            {
                return _loginBtnText;
            }
            set
            {
                Set("LoginBtnText", ref _loginBtnText, value);
            }
        }

        public RelayCommand RegisterCommand {
            get
            {
                if (_registerCommand == null)
                {
                    _registerCommand = new RelayCommand(async () =>
                    {
                        ButtonEnabled = false;
                        int result = 0;
                        Response res;
                        ErrorText = "";
                        try
                        {
                            ButtonText = "Aan het registreren...";
                            //res = await userService.GetCheckEmail(User.Email);

                            //if (res.Message == "email doesn't exist")
                            //{
                            //    //result = await userService.PostRegisterUser(User);
                            //} else
                            //{
                            //    ErrorText = "email bestaat al";
                            //}
                            
                        }
                        catch (Exception ex)
                        {
                            ButtonText = "Registratie gefaald";
                        }

                        ButtonText = "Registreren";
                        ButtonEnabled = true;
                    });
                }

                return _registerCommand;
            }
        }

        public RelayCommand SwitchUserScreen {
            get
            {
                if (_switchUserScreen == null)
                {
                    _switchUserScreen = new RelayCommand(async () =>
                    {
                        Debug.WriteLine("SWITCH_SCREEN_COMMAND");
                        if (TextBoxVisibility == Visibility.Collapsed)
                        {
                            TextBoxVisibility = Visibility.Visible;
                            LoginBtnVisibility = Visibility.Collapsed;
                            UserSwitchLinkText = "Ik heb al een account";
                        } else
                        {
                            TextBoxVisibility = Visibility.Collapsed;
                            LoginBtnVisibility = Visibility.Visible;
                            UserSwitchLinkText = "Ik heb nog geen account";
                        }
                    });
                }
                return _switchUserScreen;
            }
        }

        public RelayCommand LoginCommand {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(async () =>
                    {
                        LoginBtnText = "Gebruiker aan het aanmelden...";
                        ButtonEnabled = false;
                        ErrorText = "";

                        var res = new Response();

                        try
                        {
                            //res = await userService.PostLogin(User);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            LoginBtnText = "Aanmelden mislukt";
                        } finally
                        {
                            ButtonEnabled = true;
                        }

                        if (res.Message == "Password valid")
                        {
                            Debug.WriteLine("SWITCH TO MAINSCREEN");
                        } else
                        {
                            ErrorText = "email/wachtwoord combinatie incorrect";
                        }
                        LoginBtnText = "Aanmelden";
                    });
                }
                return _loginCommand;
            }
        }
    }
}
