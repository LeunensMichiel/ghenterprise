﻿using GalaSoft.MvvmLight;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;

namespace Ghenterprise.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private const double DefaultZoomLevel = 14;
        public ObservableCollection<Enterprise> Source { get; } = new ObservableCollection<Enterprise>();
        private EnterpriseService entService = new EnterpriseService();


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

        public MapViewModel()
        {
            Center = new Geopoint(_ghentposition);
            ZoomLevel = DefaultZoomLevel;
        }

        public async Task InitializeAsync(MapControl map)
        {
            if (map != null)
            {
                var resources = new Windows.ApplicationModel.Resources.ResourceLoader("api");
                map.MapServiceToken = resources.GetString("MapServiceToken");
                var entlist = await entService.GetEnterprisesAsync();
                entlist.ForEach(async (entie) => {
                    string addressString = $"{entie.Location.Street.Name} {entie.Location.Street_Number}, 9000 Ghent";
                    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(addressString, Center, 1);

                    if (result.Status == MapLocationFinderStatus.Success)
                    {
                        BasicGeoposition _ghenterpriseLocation = new BasicGeoposition()
                        {
                            Latitude = result.Locations[0].Point.Position.Latitude,
                            Longitude = result.Locations[0].Point.Position.Longitude
                        };
                        AddMapIcon(map, new Geopoint(_ghenterpriseLocation), entie.Name);
                    }
                });

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
