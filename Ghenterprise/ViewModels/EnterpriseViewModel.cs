using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.ViewModels
{
    public class EnterpriseViewModel:ViewModelBase
    {
        private EnterpriseService entService;

        private RelayCommand _saveEnterpriseCommand;

        private string _errorText;
        private string _buttonText = "Opslaan";
        private Boolean _buttonEnabled = true;
        private Enterprise _enterprise = new Enterprise();

        public EnterpriseViewModel()
        {
            entService = new EnterpriseService();
        }

        public Enterprise Enterprise {
            get
            {
                return _enterprise;
            }
            set
            {
                _enterprise = value;
                RaisePropertyChanged("Enterprise");
            }
        }

        public string ErrorText {
            get
            {
                return _errorText;
            }
            set
            {
                Set("ErrorText", ref _errorText, value);
            }
        }

        public string ButtonText {
            get
            {
                return _buttonText;
            }
            set
            {
                Set("ButtonText", ref _buttonText, value);
            }
        }

        public Boolean ButtonEnabled
        {
            get
            {
                return _buttonEnabled;
            }
            set
            {
                Set("ButtonEnabled", ref _buttonEnabled, value);
            }
        }

        public RelayCommand SaveEnterpriseCommand {
            get
            {
                if (_saveEnterpriseCommand == null)
                {
                    _saveEnterpriseCommand = new RelayCommand(async () =>
                    {
                        ButtonEnabled = false;
                        Boolean result = false;
                        ErrorText = "";
                        try
                        {
                            ButtonText = "Aan het opslaan...";
                            Debug.WriteLine(JsonConvert.SerializeObject(Enterprise));
                            result = await entService.SaveEnterprise(Enterprise);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            ButtonEnabled = true;
                        }
                        ButtonText = "Opslaan";
                        ButtonEnabled = true;
                        if (!result)
                        {
                            ErrorText = "Niet opgeslagen.";
                        }
                    });
                }
                return _saveEnterpriseCommand;
            }
        }
    }
}
