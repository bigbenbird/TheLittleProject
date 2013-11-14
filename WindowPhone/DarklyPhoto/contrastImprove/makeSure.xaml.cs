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
using System.Windows.Media.Imaging;
using System.IO;
namespace contrastImprove
{
    public partial class makeSure : PhoneApplicationPage
    {
        public makeSure()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            BitmapImage bmp = new BitmapImage();
            App app = Application.Current as App;
            bmp.SetSource(app.photoResult.ChosenPhoto);
            image1.Source = bmp;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GetFog.xaml", UriKind.Relative));
        }
    }
}