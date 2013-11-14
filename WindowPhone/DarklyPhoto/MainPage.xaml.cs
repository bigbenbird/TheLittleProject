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
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using System.IO;
using Microsoft.Xna.Framework.Media;
using System.Windows.Navigation;
namespace DarklyPhoto
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

                NavigationService.Navigate(new Uri("/makeSure.xaml", UriKind.Relative));
                App app = Application.Current as App;
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);
                app.photoResult = new WriteableBitmap(bitmap);

                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get a dictionary of query string keys and values.
            IDictionary<string, string> queryStrings = this.NavigationContext.QueryString;


            // Ensure that there is at least one key in the query string, and check whether the "token" key is present.
            if (queryStrings.ContainsKey("token"))
            {

                // Retrieve the picture from the media library using the token passed to the application.
                MediaLibrary library = new MediaLibrary();
                Picture picture = library.GetPictureFromToken(queryStrings["token"]);


                // Create a WriteableBitmap object and add it to the Image control Source property.
                BitmapImage bitmap = new BitmapImage();
                bitmap.CreateOptions = BitmapCreateOptions.None;
                bitmap.SetSource(picture.GetImage());

                App app = Application.Current as App;

                app.photoResult = new WriteableBitmap(bitmap);
                NavigationService.Navigate(new Uri("/makeSure.xaml", UriKind.Relative));
                queryStrings.Remove("token");
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