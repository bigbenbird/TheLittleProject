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
using System.IO.IsolatedStorage;
namespace SheldonWhip
{
    public partial class Set : PhoneApplicationPage
    {
        public Set()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            //toggleSwitch1.IsChecked = bool.Parse(settings["ApplicationIdelDetectionMode"].ToString());
            toggleSwitch1.IsChecked = !bool.Parse(settings["allow"].ToString());
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            //settings["ApplicationIdelDetectionMode"] = toggleSwitch1.IsChecked;
            settings["allow"] = !toggleSwitch1.IsChecked;
            settings.Save();
        }
    }
}