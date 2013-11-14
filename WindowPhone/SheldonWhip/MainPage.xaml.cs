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
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
namespace SheldonWhip
{
    public partial class MainPage : PhoneApplicationPage
    {
        System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();
        private DateTime currentDate = new DateTime();
        Accelerometer accelerometer;
        static  MediaElement mediaElement;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (!Accelerometer.IsSupported)
            {
                // The device on which the application is running does not support
                // the accelerometer sensor. Alert the user and disable the
                // Start and Stop buttons.
                MessageBox.Show("你手机不支持重力传感器");
            }
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (accelerometer == null)
            {
                // Instantiate the Accelerometer.
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            }
            try
            {
                accelerometer.Start();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("unable to start accelerometer.");
            }
        }
        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            // Call UpdateUI on the UI thread and pass the AccelerometerReading.
            if ((e.SensorReading.Acceleration.X > 1.8 || e.SensorReading.Acceleration.X < -1.8) && ( (DateTime.Now-currentDate)).TotalSeconds>1)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        if (MainME.Source == null)
                        {
                            MainME.Source = new Uri("whip.mp3", UriKind.Relative);
                        }
                        MainME.Position = new TimeSpan(0);
                        MainME.Play();
                        currentDate = DateTime.Now;
                        Debug.WriteLine("yes");
                    }
                );
            }
            
        }
        private void MainME_MediaOpened(object sender, RoutedEventArgs e) 
        {
            //MainME.Stop();
            MainME.Play(); 
        }
        private void MainME_MediaFailed(object sender, RoutedEventArgs e)
        {
            MainME.Stop();
            Debug.WriteLine(e.ToString());
        
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
        private void ApplicationBarMenuItem2_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Set.xaml", UriKind.Relative));
        }
    }

}