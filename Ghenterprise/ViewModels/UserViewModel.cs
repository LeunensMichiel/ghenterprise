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
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace Ghenterprise.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private User _user;
        private BitmapImage _photo;
        private bool _isLoggedIn;
        private bool _isAuthorized;


        private UserService UserService = new UserService();

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => Set(ref _isLoggedIn, value);
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { Set(ref _isAuthorized, value); }
        }

        public User User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        public BitmapImage Photo
        {
            get => _photo;
            set => Set(ref _photo, value);
        }

        public UserViewModel()
        {
            IsLoggedIn = false;
            IsAuthorized = false;
            Photo = new BitmapImage(new Uri("ms-appx:///Assets/DefaultIcon.png"));
            User = new User();
        }

        public async Task LogUserIn(User user)
        {
            User dbUser = await UserService.PostLogin(user);
            if (dbUser != null)
            {
                User = dbUser;
                IsLoggedIn = true;
                IsAuthorized = true;
            }
        }

        public void LogUserOut()
        {
            UserService.RemoveToken(User);
            User = new User();
            IsLoggedIn = false;
            IsAuthorized = false;
        }
    }
}
