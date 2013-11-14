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
using System.IO.IsolatedStorage;
using System.Device.Location;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.ServiceModel;
using System.Text;
namespace SMSOS
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher watcher;
        private string[] phones;
        private string gSearchURL = "http://maps.googleapis.com/maps/api/geocode/json?language=zh-CH&latlng=";
        private string latlng;
        static bool allow;
        public MainPage()
        {
            InitializeComponent();
            
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                string from = NavigationContext.QueryString["from"];
                radioButton1.Content = NavigationContext.QueryString["1"];
                radioButton2.Content = NavigationContext.QueryString["2"];
                radioButton3.Content = NavigationContext.QueryString["3"];
                radioButton4.Content = NavigationContext.QueryString["4"];
                radioButton5.Content = NavigationContext.QueryString["5"];
                radioButton6.Content = NavigationContext.QueryString["6"];
                radioButton7.Content = NavigationContext.QueryString["7"];
                radioButton8.Content = NavigationContext.QueryString["8"];
                radioButton9.Content = NavigationContext.QueryString["9"];
            }
            catch
            {
                //throw;
            }
        
        }

        //页面载入，从独立存储中载入预设亲友列表
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("FriendsPhoneList"))
            {
                phones = (string[])IsolatedStorageSettings.ApplicationSettings["FriendsPhoneList"];
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("你的亲友列表为空，是否预设亲友号码？取消将退出程序", "欢迎", MessageBoxButton.OKCancel);
                if (mbr == MessageBoxResult.OK)
                {
                    NavigationService.Navigate(new Uri("/SetFriends.xaml", UriKind.Relative));
                }
                else
                {
                    while (NavigationService.BackStack.Any())
                        NavigationService.RemoveBackEntry();
                    NavigationService.GoBack();
                }
            }
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings.TryGetValue<bool>("allow", out allow);
            if (allow == true)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.Start();
            }
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                GeoCoordinate currentPosition = watcher.Position.Location;
                txbPosition.Text = string.Format("当前位置：经度-{0} | 纬度-{1}", currentPosition.Longitude, currentPosition.Latitude);
                latlng = string.Format("{0},{1}&sensor=true", currentPosition.Latitude,currentPosition.Longitude);
                Debug.WriteLine(latlng);
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(google_search_completed);
                webClient.DownloadStringAsync(new Uri(gSearchURL + latlng));
                watcher.Stop();
            }
        }
        private void google_search_completed(object sender,DownloadStringCompletedEventArgs e)
        {
            try
            {
                int a = e.Result.IndexOf("formatted_address");
                int b = e.Result.IndexOf("geometry", a);
                Debug.WriteLine((b - a).ToString());
                string adress = e.Result.Substring(a + 21, (b - a) - 33);
                textBlock1.Text = adress;
            }
            catch
            { }
        }

        //短信求援
        private void btnSms_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask smsCompose = new SmsComposeTask();
            string body;
            string message="";
            if (radioButton1.IsChecked==true)
                message = string.Format("{0}{1}", message, radioButton1.Content);
            if (radioButton2.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton2.Content);
            if (radioButton3.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton3.Content);
            if (radioButton4.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton4.Content);
            if (radioButton5.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton5.Content);
            if (radioButton6.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton6.Content);
            if (radioButton7.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton7.Content);
            if (radioButton8.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton8.Content);
            if (radioButton9.IsChecked == true)
                message = string.Format("{0}{1}", message, radioButton9.Content);
 
            body = string.Format("Help Me!\n我现在在{0},位置是{1},发生了{2},请帮忙赶快联系警察或来救我！！", txbPosition.Text, textBlock1.Text, message);
            smsCompose.Body = "Help Me!\n" +body;
            smsCompose.To = string.Join(",", phones);
            smsCompose.Show();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SetFriends.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {

            string s = string.Format("/SetEvent.xaml?1={0}&2={1}&3={2}&4={3}&5={4}&6={5}&7={6}&8={7}&9={8}",
                radioButton1.Content,radioButton2.Content,radioButton3.Content,radioButton4.Content,radioButton5.Content,
                radioButton6.Content,radioButton7.Content,radioButton8.Content,radioButton9.Content);
            NavigationService.Navigate(new Uri(s,UriKind.Relative));
        }
        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
        private void ApplicationBarIconButton_Click3(object sender, EventArgs e)
        {
            IsolatedStorageSettings settings= IsolatedStorageSettings.ApplicationSettings;
            settings.TryGetValue<bool>("allow",out allow);
            if (allow == true)
            {
                allow = false;
                MessageBox.Show("已经禁止使用位置定位信息,下一次启动开始有效");
            }
            else
            {
                allow = true;
                MessageBox.Show("已经开启位置定位信息,下一次启动开始有效");
            }
            settings["allow"] = allow;
            settings.Save();
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            //PhoneApplicationPage_Loaded(sender,e);
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
    }
    
    //[DataContract]
    //public  class adress
    //{
    //    [DataMember]
    //    public GAddressGeoResult results { get; set; }
    //    [DataMember]
    //    public string status {get;set;}
    //}

    //[DataContract]
    //public class GAddressGeoResult 
    //{
    //    [DataMember]
    //    public string formatted_address {get;set;}
    //}

}
