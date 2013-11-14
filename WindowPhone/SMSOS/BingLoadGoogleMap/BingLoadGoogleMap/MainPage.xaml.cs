using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;

namespace BingLoadGoogleMap
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            MapTileLayer customTileLayer = new MapTileLayer();
            BingLoadGoogleMap.GoogleTileSource gts = new BingLoadGoogleMap.GoogleTileSource();
            customTileLayer.TileSources.Add(gts);
            myMap.ZoomLevel = 15;
            myMap.Center = new System.Device.Location.GeoCoordinate(31.2351889908314, 121.483741924167);
        }
    }
}