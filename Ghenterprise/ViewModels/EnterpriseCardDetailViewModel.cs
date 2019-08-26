using GalaSoft.MvvmLight;
using Ghenterprise.Data;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls.Maps;

namespace Ghenterprise.ViewModels
{
    public class EnterpriseCardDetailViewModel : ViewModelBase
    {
        private const double DefaultZoomLevel = 14;

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

        public EnterpriseService EnterpriseService { get; set; }

        public EnterpriseCardDetailViewModel()
        {
            Center = new Geopoint(_ghentposition);
            ZoomLevel = DefaultZoomLevel;
            EnterpriseService = new EnterpriseService();
        }

        public async Task InitializeAsync(string Id, MapControl map)
        {
            try
            {
                var items = await EnterpriseService.GetEnterpriseAsync(Id);
                Enterprise = items.First();

                if (map != null)
                {
                    var resources = new Windows.ApplicationModel.Resources.ResourceLoader("api");
                    map.MapServiceToken = resources.GetString("MapServiceToken");
                    AddMapIcon(map, Center, Enterprise.Name);
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
