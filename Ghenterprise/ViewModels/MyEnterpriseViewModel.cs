using GalaSoft.MvvmLight;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.ViewModels
{
    public class MyEnterpriseViewModel : ViewModelBase
    {
        private Enterprise _selected;
        private EnterpriseService entService = new EnterpriseService();
        private List<Enterprise> _entlist = new List<Enterprise>();

        public Enterprise Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Enterprise> Source { get; private set; } = new ObservableCollection<Enterprise>();
       

        public MyEnterpriseViewModel()
        {

        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            _entlist = await entService.GetEnterprisesAsync();

            _entlist.ForEach(ent => { Source.Add(ent); });



            //Enterprise enti = new Enterprise();
            //enti.Name = "Leunes Media";
            //enti.Description = "Fotografie / Webdev";
            //enti.Id = "AFER";
            //enti.Date_Created = new DateTime();
            //Source.Add(enti);

            //Enterprise enti2 = new Enterprise();
            //enti2.Name = "Kastart";
            //enti2.Description = "Fuck ik heb honger";
            //enti2.Id = "QWERTY";
            //enti2.Date_Created = new DateTime();
            //Source.Add(enti2);

            /*var data = await SampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }*/

            if (viewState == MasterDetailsViewState.Both)
            {
                if (Source.Count > 0)
                    Selected = Source.First();
            }
        }
    }
}
