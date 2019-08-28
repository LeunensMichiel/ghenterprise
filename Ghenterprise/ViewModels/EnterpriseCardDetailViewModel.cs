using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;

namespace Ghenterprise.ViewModels
{
    public class EnterpriseCardDetailViewModel : ViewModelBase
    {
        private const double DefaultZoomLevel = 14;
        private string addressString = "";
        private ICommand _toggled;
        public ICommand ToggledCommand => _toggled ?? (_toggled = new RelayCommand(new Action(Toggled)));
        public ObservableCollection<Event> EventsSource { get; } = new ObservableCollection<Event>();
        public ObservableCollection<Promotion> PromoSource { get; } = new ObservableCollection<Promotion>();

        private void Toggled()
        {
            if (IsOn == true)
            {
                SubscribeToEvent();
            }
            else
            {
                UnsubscribeToEvent();
            }
        }

        public UserViewModel UserViewModel => ViewModelLocator.Current.User;

        private string _openClosedString = "Geen openingsuren beschikbaar";
        public string OpenClosedString
        {
            get => _openClosedString;
            set
            {
                _openClosedString = value;
                RaisePropertyChanged("OpenClosedString");
            }
        }

        private bool _ison = false;
        public bool IsOn
        {
            get => _ison;
            set
            {
                Set(ref _ison, value);
            }
        }

        private async Task UnsubscribeToEvent()
        {
            try
            {
                await EnterpriseService.UnSubscribeToEnterprise(Enterprise.Id);
                IsOn = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async Task SubscribeToEvent()
        {
            try
            {
                await EnterpriseService.SubscribeToEnterprise(Enterprise.Id);
                IsOn = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);  
            }
        }

        private readonly BasicGeoposition _ghentposition = new BasicGeoposition()
        {
            Latitude = 51.05563,
            Longitude = 3.72856
        };

        private double _zoomLevel;

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Set(ref _zoomLevel, value); }
        }

        private Geopoint _center;

        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        private Enterprise _enterprise;
        public Enterprise Enterprise
        {
            get { return _enterprise; }
            set { Set(ref _enterprise, value); }
        }

        private List<Enterprise> _subscriptionlist = new List<Enterprise>();


        public EnterpriseService EnterpriseService { get; set; }

        public EnterpriseCardDetailViewModel()
        {
            Center = new Geopoint(_ghentposition);
            ZoomLevel = DefaultZoomLevel;
            EnterpriseService = new EnterpriseService();
            IsOn = false;
        }

        public async Task InitializeAsync(string Id, MapControl map)
        {
            try
            {
                var items = await EnterpriseService.GetEnterpriseAsync(Id);
                Enterprise = items.FirstOrDefault();
                //Enterprise.Events.ForEach(e => EventsSource.Add(e));
                //Enterprise.Promotions.ForEach(e => PromoSource.Add(e));


                var items2 = await EnterpriseService.GetSubscriptionsAsync();
                if (items2.Any(i => i.Id.Equals(Enterprise.Id)))
                {
                    IsOn = true;
                } else
                {
                    IsOn = false;
                }

                if (map != null)
                {
                    addressString = $"{Enterprise.Location.Street.Name} {Enterprise.Location.Street_Number}, 9000 Ghent";
                    var resources = new Windows.ApplicationModel.Resources.ResourceLoader("api");
                    map.MapServiceToken = resources.GetString("MapServiceToken");

                    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(addressString, Center, 1);

                    if (result.Status == MapLocationFinderStatus.Success)
                    {
                        BasicGeoposition _ghenterpriseLocation = new BasicGeoposition()
                        {
                            Latitude = result.Locations[0].Point.Position.Latitude,
                            Longitude = result.Locations[0].Point.Position.Longitude
                        };
                        Geopoint newCenter = new Geopoint(_ghenterpriseLocation);
                        AddMapIcon(map, newCenter, Enterprise.Name);
                        Center = newCenter;
                    }
                    else
                    {
                        AddMapIcon(map, Center, "Geen locatie gevonden");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

        }

        private void AddMapIcon(MapControl map, Geopoint position, string title)
        {
            var mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,
                ZIndex = 0
            };
            map.MapElements.Add(mapIcon);
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
