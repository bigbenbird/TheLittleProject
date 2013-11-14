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
using System.IO.IsolatedStorage;
using System.Collections;
using System.Diagnostics;
namespace RunFun
{
    
    public partial class Run : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        DateTimeOffset start;
        AccelerometerReading pre;
        App app;
        int n;
        public Run()
        {
            InitializeComponent();
            if (!Accelerometer.IsSupported)
            {
                // The device on which the application is running does not support
                // the accelerometer sensor. Alert the user and disable the
                // Start and Stop buttons.
                startButton.IsEnabled = false;
                stopButton.IsEnabled = false;
                MessageBox.Show("你手机不支持重力传感器");
            }
            app = Application.Current as App; 
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            n = 0;
            if (app.list == null)
            {
                //app.list = new List<DateTimeOffset>();
                app.list=new List<DateTimeOffset>();
            }
            else if (app.list.Count != 0)
            {
                app.list.Clear();
            }

        }
        private void startButton_Click(object sender, RoutedEventArgs e)
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
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
            SaveChange(e.SensorReading);

        }
        private void SaveChange(AccelerometerReading accelerometerReading)
        {
            if (n == 0)
            {
                pre = accelerometerReading;
                start = pre.Timestamp;
                n++;
                app.list.Add(start);
                return;
            }
            if (accelerometerReading.Acceleration.Y * pre.Acceleration.Y < 0)
            {
                double second=(accelerometerReading.Acceleration.Y*((pre.Timestamp-start).TotalSeconds))-((accelerometerReading.Timestamp-start).TotalSeconds)*pre.Acceleration.Y;
                second /= accelerometerReading.Acceleration.Y - pre.Acceleration.Y;
                if (second < 0)
                    Debug.WriteLine("second<0",second.ToString());
                if((1 / (start.AddSeconds(second) - app.list[app.list.Count-1]).TotalSeconds)<7)
                    app.list.Add(start.AddSeconds(second));
            }
            pre = accelerometerReading;
            //app.list.Add(accelerometerReading);
            //Debug.WriteLine("Time:" + accelerometerReading.Timestamp.Second.ToString()+":"+accelerometerReading.Timestamp.Millisecond.ToString() + "  g:" + accelerometerReading.Acceleration.Y.ToString());

        }
        private void UpdateUI(AccelerometerReading accelerometerReading)
        {
            //MessageBox.Show("getting data");

            Vector3 acceleration = accelerometerReading.Acceleration;

            // Show the numeric values.
            textBox1.Text = "Y: " + acceleration.Y.ToString("0.00");

            // Show the values graphically.
            yLine.Y2 = yLine.Y1 - acceleration.Y * 250;
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (accelerometer != null)
            {
                // Stop the accelerometer.
                accelerometer.Stop();
                MessageBox.Show("测算依据结束");
                NavigationService.Navigate(new Uri("/frequency.xaml", UriKind.Relative));
            }
        }
    }
}