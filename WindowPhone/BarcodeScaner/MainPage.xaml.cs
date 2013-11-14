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
using Microsoft.Phone.Shell;
using com.google.zxing;
using WP7_Barcode_Library;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
namespace BarcodeScaner
{
    public partial class MainPage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }
        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            if (e != null && e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                bitmapImage.SetSource(e.ChosenPhoto);
                WP7_Barcode_Library.WP7BarcodeManager.ScanBarcode(bitmapImage, new Action<BarcodeCaptureResult>(Barcode_Results));
            }
            
        
        }
        public void start_progress()
        {
            pbStatus.IsIndeterminate = true;
        }

        public void stop_progress()
        {
            pbStatus.IsIndeterminate = false;
        }
        private void lpBarcodeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //NOTE: code will be called when page is loaded before lpBarcodeType is initialize and may be called without a selected item
            if (lpBarcodeType != null && lpBarcodeType.SelectedIndex != -1)
            {
                BarcodeTypeItem selectedItem = lpBarcodeType.SelectedItem as BarcodeTypeItem;
                WP7BarcodeManager.ScanMode = selectedItem.BarcodeType; //Tell barcode library what type of scan we want to perform
            }
            if (((sender == lpBarcodeType) && (WP7BarcodeManager.LastCaptureResults.BarcodeImage != null)) && ((WP7BarcodeManager.LastCaptureResults.State == WP7_Barcode_Library.CaptureState.ScanFailed) || (WP7BarcodeManager.LastCaptureResults.State == WP7_Barcode_Library.CaptureState.UnknownError)))
            {
                start_progress(); //Try scanning previously failed image again using new selected mode
                WP7BarcodeManager.ScanBarcode(WP7BarcodeManager.LastCaptureResults.BarcodeImage, new Action<BarcodeCaptureResult>(Barcode_Results));
            }
        }
        public void Barcode_Results(WP7_Barcode_Library.BarcodeCaptureResult Results)
        {
            if (Results.BarcodeImage != null)
            {
                imgCapture.Source = Results.BarcodeImage; //Display image
            }
            else
            {
                //No image captured
            }
            if (Results.State == WP7_Barcode_Library.CaptureState.Success)
            {
                txtResults.Text = Results.BarcodeText; //Use results
            }
            else //Error occured
            {
                txtResults.Text = "发生错误: 请确认编码形式正确或者图片清晰并再试一次";
            }
            stop_progress();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
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
                    WP7_Barcode_Library.WP7BarcodeManager.ScanBarcode(bitmap, Barcode_Results);

                    //bitmap.CreateOptions = BitmapCreateOptions.None;
                    //bitmap.SetSource(picture.GetImage());
                }

                WP7BarcodeManager.aStartProgress = start_progress;

                if (PhoneApplicationService.Current.State.Keys.Contains("ReturnFromCameraCapture"))
                {
                    //You can also detect if we are returning from the camera and perform camera specific code here.
                    PhoneApplicationService.Current.State.Remove("ReturnFromCameraCapture"); //Remove flag so code doesn't execute multiple times
                }
                else //Initial page load (not from camera or sample chooser)
                {
                    lpBarcodeType_SelectionChanged(null, null); //Setup barcode type used for scanning
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error processing sample image.", ex);
            }



        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            start_progress();
            WP7BarcodeManager.ScanBarcode(Barcode_Results);
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Detect if page is navigating back from BarcodeSampleChooser and start processing
            try
            {
                WP7BarcodeManager.aStartProgress = start_progress;

                if (PhoneApplicationService.Current.State.Keys.Contains("ReturnFromSampleChooser"))
                {
                    PhoneApplicationService.Current.State.Remove("ReturnFromSampleChooser");//Remove flag so code doesn't execute multiple times
                    if (BarcodeSampleItemManager.SelectedItem != null)
                    {
                        WP7BarcodeManager.ScanBarcode(BarcodeSampleItemManager.SelectedItem.FileURI, new Action<BarcodeCaptureResult>(this.Barcode_Results));
                    }
                    else //User backed out of the sample chooser without picking an image
                    {
                        MessageBox.Show("Error: Sample chooser canceled");
                    }
                }
                else if (PhoneApplicationService.Current.State.Keys.Contains("ReturnFromCameraCapture"))
                {
                    //You can also detect if we are returning from the camera and perform camera specific code here.
                    PhoneApplicationService.Current.State.Remove("ReturnFromCameraCapture"); //Remove flag so code doesn't execute multiple times
                }
                else //Initial page load (not from camera or sample chooser)
                {
                    lpBarcodeType_SelectionChanged(null, null); //Setup barcode type used for scanning
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error processing sample image.", ex);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
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

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/about.xaml",UriKind.Relative));
        }
        
    }
}