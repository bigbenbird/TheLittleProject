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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Devices.Sensors;

namespace RunFun
{
    public partial class App : Application
    {
        public List<DateTimeOffset> list { get; set; }
        //public List<AccelerometerReading> list { get; set; }
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("firstTime"))
            {
                //Boolean b;
                //settings.TryGetValue<Boolean>("ApplicationIdelDetectionMode", out b);
                //if (b == false)
                //{
                //    MessageBox.Show("因为你不允许锁屏运行，所以请在不锁屏状态下运行本软件");
                //}
                //else
                //{
                //    MessageBox.Show("你已经选择了锁屏下运行，可以在设置里面修改");
                //}
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("您是第一次使用本App,是否开启运行时不锁屏功能", "Welcome", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                {
                    //settings.Add("ApplicationIdelDetectionMode",true);
                    settings.Add("UserIdleDetectionMode", true);
                }
                else
                {
                    //settings.Add("ApplicationIdelDetectionMode",false);
                    ////MessageBoxResult mbr2 = MessageBox.Show("您不开启开启锁屏运行,是否开启应用运行时不锁屏", "Welcome", MessageBoxButton.OKCancel);
                    //if (mbr2 == MessageBoxResult.OK)
                    //{
                    //    settings.Add("UserIdleDetectionMode", true);
                    //}
                    //else
                    //{
                        settings.Add("UserIdleDetectionMode", false);
                        MessageBox.Show("应用只能在不锁屏的情况下运行，请注意！");
                    //}

                }

                settings.Add("firstTime", false);
                settings.Save();
                //if (bool.Parse(settings["ApplicationIdelDetectionMode"].ToString()))
                //    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
                //else
                //    PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Enabled;

                if (bool.Parse(settings["UserIdleDetectionMode"].ToString()))
                { 
                    PhoneApplicationService.Current.UserIdleDetectionMode=IdleDetectionMode.Disabled;

                }
                else
                    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}