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
using Microsoft.Phone.Shell;
namespace RunFun
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
            toggleSwitch2.IsChecked = bool.Parse(settings["UserIdleDetectionMode"].ToString());
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            //settings["ApplicationIdelDetectionMode"] = toggleSwitch1.IsChecked;
            settings["UserIdleDetectionMode"] = toggleSwitch2.IsChecked;
            settings.Save();

            bool b;
            //settings.TryGetValue<bool>("ApplicationIdelDetectionMode",out b);
            //if (!b)
            //    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
            //else
            //    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Enabled;

            settings.TryGetValue<bool>("UserIdleDetectionMode", out b);
            if (!b)
            {
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;

            }
            else
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;

        }
    }
}