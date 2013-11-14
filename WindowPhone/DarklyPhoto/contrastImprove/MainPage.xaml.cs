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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace contrastImprove
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask cameraCaptureTask;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
           
        }
        private void OnAppBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar.BackgroundColor = e.IsMenuVisible ? Colors.Black : Colors.Transparent;
        }
        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
            //NavigationService.GoBack();
            MessageBoxResult msgRst = MessageBox.Show("要退出本程序吗？", "提示", MessageBoxButton.OKCancel);
            if (msgRst == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                photoChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred.");
            }

        }
        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show(e.ChosenPhoto.Length.ToString());

                NavigationService.Navigate(new Uri("/makeSure.xaml",UriKind.Relative));
                App app = Application.Current as App;
                app.photoResult = e;
                
                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cameraCaptureTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred.");
            }
        }

        private void Set_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Set.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
        //void cameraCaptureTask_Completed(object sender, PhotoResult e)
        //{
        //    if (e.TaskResult == TaskResult.OK)
        //    {
        //        MessageBox.Show(e.ChosenPhoto.Length.ToString());

        //        //Code to display the photo on the page in an image control named myImage.
        //        //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
        //        //bmp.SetSource(e.ChosenPhoto);
        //        //myImage.Source = bmp;
        //    }
        //}
    }
}