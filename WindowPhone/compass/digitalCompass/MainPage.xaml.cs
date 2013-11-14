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
using System.Windows.Threading;

namespace digitalCompass
{
    public partial class MainPage : PhoneApplicationPage
    {

        Compass compass;
        DispatcherTimer timer;
        Accelerometer accelerometer;

        double centerX = 185;
        double centerY = 185;
        double magneticHeading;
        double trueHeading;
        double headingAccuracy;
        Vector3 rawMagnetometerReading;
        bool isDataValid;
        bool calibrating = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (!Compass.IsSupported)
            {
                // The device on which the application is running does not support
                // the compass sensor. Alert the user and hide the
                // application bar.
                MessageBox.Show("设备不支持罗盘传感器");
                ApplicationBar.IsVisible = false;
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(30);
                timer.Tick += new EventHandler(timer_Tick);
            }
        }
        //}
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (compass != null && compass.IsDataValid)
            {
                // Stop data acquisition from the compass.
                compass.Stop();
                timer.Stop();
                accelerometer.Stop();
                MessageBox.Show("指南针停止");
            }
              else
            {
                if (compass == null)
                {
                        // Instantiate the compass.
                    compass = new Compass();

                    // Specify the desired time between updates. The sensor accepts
                       // intervals in multiples of 20 ms.
                    compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);

                        // The sensor may not support the requested time between updates.
                            // The TimeBetweenUpdates property reflects the actual rate.
                    compass.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<CompassReading>>(compass_CurrentValueChanged);
                    compass.Calibrate += new EventHandler<CalibrationEventArgs>(compass_Calibrate);
                }
                try
                {
                    MessageBox.Show ("成功启动传感器");
                    compass.Start();
                    timer.Start();


                    // Start accelerometer for detecting compass axis
                    accelerometer = new Accelerometer();
                    accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
                    accelerometer.Start();
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show ( "打开罗盘传感器失败");
                }

            }
        }
        void compass_CurrentValueChanged(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            // Note that this event handler is called from a background thread
            // and therefore does not have access to the UI thread. To update 
            // the UI from this handler, use Dispatcher.BeginInvoke() as shown.
            // Dispatcher.BeginInvoke(() => { statusTextBlock.Text = "in CurrentValueChanged"; });


            isDataValid = compass.IsDataValid;

            trueHeading = e.SensorReading.TrueHeading;
            magneticHeading = e.SensorReading.MagneticHeading;
            headingAccuracy = Math.Abs(e.SensorReading.HeadingAccuracy);
            rawMagnetometerReading = e.SensorReading.MagnetometerReading;

        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (!calibrating)
            {
                if (isDataValid)
                {
                    statusTextBlock.Text = "正在接受数据";
                }
                magneticLine.X2 = centerX - centerY * Math.Sin(MathHelper.ToRadians((float)magneticHeading));
                magneticLine.Y2 = centerY - centerY * Math.Cos(MathHelper.ToRadians((float)magneticHeading));
                trueLine.X2 = centerX - centerY * Math.Sin(MathHelper.ToRadians((float)trueHeading));
                trueLine.Y2 = centerY - centerY * Math.Cos(MathHelper.ToRadians((float)trueHeading));
            }
            else
            {
                if (headingAccuracy <= 10)
                {
                    calibrationTextBlock.Foreground = new SolidColorBrush(Colors.Green);
                    calibrationTextBlock.Text = "Complete!";
                }
                else
                {
                    calibrationTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                    calibrationTextBlock.Text = headingAccuracy.ToString("0.0");
                }
            }
        }
        void compass_Calibrate(object sender, CalibrationEventArgs e)
        {
            Dispatcher.BeginInvoke(() => { calibrationStackPanel.Visibility = Visibility.Visible; });
            calibrating = true;
        }
        private void calibrationButton_Click(object sender, RoutedEventArgs e)
        {
            calibrationStackPanel.Visibility = Visibility.Collapsed;
            calibrating = false;
        }
        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Vector3 v = e.SensorReading.Acceleration;

            bool isCompassUsingNegativeZAxis = false;

            if (Math.Abs(v.Z) < Math.Cos(Math.PI / 4) &&
                          (v.Y < Math.Sin(7 * Math.PI / 4)))
            {
                isCompassUsingNegativeZAxis = true;
            }

            Dispatcher.BeginInvoke(() => { orientationTextBlock.Text = (isCompassUsingNegativeZAxis) ? "纵向模式" : "横向模式"; });
        }
        private void ApplicationBarIconButton2_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml",UriKind.Relative));
        }

    }

}