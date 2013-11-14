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
using Visifire.Charts;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
namespace RunFun
{
    public partial class frequency : PhoneApplicationPage
    {
        ObservableCollection<KeyValuePair<DateTime, Double>> Data
        {
            get;
            set;
        }
        public frequency()
        {
            InitializeComponent();

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            App app;
            app = Application.Current as App;
            if (Data == null)
                Data = new ObservableCollection<KeyValuePair<DateTime, double>>();
            else
                Data.Clear();
            TimeSpan max=new TimeSpan();
            double max_rate = 0;
            for(int i=1;i<app.list.Count;i++)
            {
                TimeSpan timeSpan=app.list[i]-app.list[0];
                
                //DateTime dateTime=new DateTime(1,1,1,timeSpan.Hours,timeSpan.Minutes,timeSpan.Seconds,timeSpan.Milliseconds);
                //Data.Add(new KeyValuePair<DateTime, double>(dateTime,1/(app.list[i+1]-app.list[i]).TotalSeconds));
                //Data.Add(new KeyValuePair<DateTime, double>(dateTime,app.list[i].Acceleration.Y));
                //MyChart.AxesX[0].Interval = (app.list[app.list.Count-1].Timestamp-app.list[0].Timestamp).TotalSeconds/4;
                Debug.WriteLine("Debug start");
                Debug.WriteLine(timeSpan.Milliseconds.ToString());
                DateTime dateTime = new DateTime(1, 1, 1, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                double rate=(1 / (app.list[i] - app.list[i - 1]).TotalSeconds);
                if(rate>max_rate)
                {
                    max_rate=rate;
                    max=timeSpan;
                }
                Data.Add(new KeyValuePair<DateTime, double>(dateTime, rate));
                MyChart.AxesX[0].Interval = (app.list[app.list.Count - 1] - app.list[0]).TotalSeconds / 4;
            }
            MyChart.Series[0].DataSource = Data;
            string str1 = String.Format("{0:F}", max_rate);
            textBlock5.Text = str1;
            textBlock6.Text = ((app.list.Count-3)).ToString();
            string value = String.Format("{0} {1}:{2}:{3}.{4:F}", max.Days, max.Hours, max.Minutes, max.Seconds,max.Milliseconds);
            textBlock9.Text = value;
            //textBlock9.Text = max.ToString();
           // MyChart.DataContext = Data;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = 1;
            transform.ScaleY = 0.5;
            WriteableBitmap wbmp = new WriteableBitmap(this, transform);

            String fileName = "TempJPEG";
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(fileName);
            Uri uri = new Uri("RunFun;component/TempJPEG.jpg", UriKind.Relative);
            //WriteableBitmap wb = new WriteableBitmap(noFog);
            Extensions.SaveJpeg(wbmp, myFileStream, wbmp.PixelWidth, wbmp.PixelHeight, 0, 90);
            myFileStream.Close();

            myFileStream = myStore.OpenFile(fileName, FileMode.Open, FileAccess.Read);
            MediaLibrary lib = new MediaLibrary();
            Picture pic = lib.SavePicture("SavePictre.jpg", myFileStream);
            myFileStream.Close();
            MessageBox.Show("图片已被保存为");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int s = int.Parse(textBox1.Text);
                App app;
                app = Application.Current as App;
                textBlock7.Text = ((s + 0.0) / app.list.Count).ToString();
            }
            catch
            {
                MessageBox.Show("请输入有效的距离");
            }
        }
    }

}