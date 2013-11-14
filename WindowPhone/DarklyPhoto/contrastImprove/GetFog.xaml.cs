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
using System.Threading;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using System.Windows.Data;
namespace contrastImprove
{
    public partial class GetFog : PhoneApplicationPage
    {
        #region ProgressIndicatorIsVisibleProperty
        public static readonly DependencyProperty ProgressIndicatorIsVisibleProperty =
            DependencyProperty.Register("ProgressIndicatorIsVisible",
            typeof(bool),
            typeof(GetFog),
            new PropertyMetadata(false));

        public bool ProgressIndicatorIsVisible
        {
            get { return (bool)GetValue(ProgressIndicatorIsVisibleProperty); }
            set { SetValue(ProgressIndicatorIsVisibleProperty, value); }
        }
        #endregion
        BitmapImage bmp;
        WriteableBitmap wb;
        App app;

        WriteableBitmap newImage;
        private int[] pixelR=new int[256];
        private int[] pixelG=new int[256];
        private int[] pixelB = new int[256];

        private double[] precentR = new double[256];
        private double[] precentG = new double[256];
        private double[] precentB = new double[256];

        private double[] tempR = new double[256];
        private double[] tempG = new double[256];
        private double[] tempB = new double[256];

        private int[] nsgR = new int[256];
        private int[] nsgG = new int[256];
        private int[] nsgB = new int[256];

        private byte airDrak;
        private double[,] darkChannelValue;
        WriteableBitmap noFog;
        WriteableBitmap darkImage;
        delegate void ProgressDelegate(int i);
        ProgressDelegate progressDelegate; 
        //声明委托类型
        //委托的内容如有不明白,见http://www.pocketdigi.com/20110916/476.html 有详细注解

        public GetFog()
        {
            InitializeComponent();
            bmp = new BitmapImage();
            app = Application.Current as App;
            
            bmp.SetSource(app.photoResult.ChosenPhoto);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            //bmp.SetSource(app.photoResult.ChosenPhoto);
            
            image1.Source = bmp;
            progressDelegate = DoSomeThing;
            button1.IsEnabled = false;
            new Thread(new ThreadStart(ThreadProc)).Start();

            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.Text = "图像处理中，请稍等";
            Binding bindingData;
            bindingData = new Binding("ProgressIndicatorIsVisible");
            bindingData.Source = this;
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsVisibleProperty, bindingData);
            BindingOperations.SetBinding(SystemTray.ProgressIndicator, ProgressIndicator.IsIndeterminateProperty, bindingData);
            ProgressIndicatorIsVisible = true;
            //progressBar1.Value = 20;
            //Thread.Sleep(1000);
            //makeDarkChannel();
            //progressBar1.Value = 50;
            //removeFog();
            //progressBar1.Value = 100;
            //image1.Source = noFog;
            //progressBar1.Visibility = Visibility.Collapsed;
        }
        private void ThreadProc()
        {
            
            this.Dispatcher.BeginInvoke(progressDelegate,10);
            //Thread.Sleep(1000);
            this.Dispatcher.BeginInvoke(progressDelegate, 70);
            //Thread.Sleep(1000);
            this.Dispatcher.BeginInvoke(progressDelegate, 100);
            //Thread.Sleep(1000);
            Deployment.Current.Dispatcher.BeginInvoke(() => ProgressIndicatorIsVisible = false);
        }
        private  void DoSomeThing(int i)
        {
            //progressBar1.Value = i;
            //Thread.Sleep(10000);
            if (i == 10)
            {
                do_precent();
                //image1.Source = darkImage;
            }
            //progressBar1.Value = 50;
            if (i == 70)
                do_aggregate();
            if (i == 100)
            {
                improve();
                image1.Source = newImage;
                //progressBar1.Value = 100;
                progressBar1.Visibility = Visibility.Collapsed;
                button1.IsEnabled = true;
            }
        }
        
        
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            String fileName = "TempJPEG";

            //BitmapImage bmp = new BitmapImage();
            //App app = Application.Current as App;
            //bmp.SetSource(app.photoResult.ChosenPhoto);

            var myStore = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream myFileStream = myStore.CreateFile(fileName);
            Uri uri = new Uri("NoFog;component/TempJPEG.jpg",UriKind.Relative);
            WriteableBitmap wb= new WriteableBitmap(noFog);
            Extensions.SaveJpeg(wb,myFileStream,wb.PixelWidth,wb.PixelHeight,0,90);
            myFileStream.Close();

            myFileStream = myStore.OpenFile(fileName,FileMode.Open,FileAccess.Read);
            MediaLibrary lib = new MediaLibrary();
            Picture pic = lib.SavePicture("SavePictre.jpg",myFileStream);
            myFileStream.Close();
            MessageBox.Show("图片已被保存为");
            //SaveToMediaLib();
        }

        private void do_precent()
        {
            wb = new WriteableBitmap(bmp);
            int[] ImageData = wb.Pixels;

            for (int i = 0; i < 256; i++)
            {
                pixelR[i] = 0;
                pixelG[i] = 0;
                pixelB[i] = 0;
            }
            for (int i = 0; i < wb.PixelHeight; i++)
                for (int j = 0; j < wb.PixelWidth; j++)
                {
                    int pixel = ImageData[i * wb.PixelWidth + j];

                    //byte AValue = (byte)((pixel & 0xff) >> 24);
                    byte RedValue = (byte)((pixel & 0x00FF0000) >> 16);
                    byte GreenValue = (byte)((pixel & 0x0000FF00) >> 8);
                    byte BlueValue = (byte)(pixel & 0x000000FF);

                    pixelR[RedValue]++;
                    pixelG[GreenValue]++;
                    pixelB[BlueValue]++;
                }
            int count=wb.PixelWidth*wb.PixelHeight;
            for (int i = 0; i < 256; i++)
            {
                precentR[i] = (pixelR[i] + 0.0) / (count);
                precentG[i] = (pixelG[i] + 0.0) / (count);
                precentB[i] = (pixelB[i] + 0.0) / (count);
            }
        }

        private void do_aggregate()
        {
            tempR[0] = precentR[0];
            tempG[0] = precentG[0];
            tempB[0] = precentB[0];

            for (int i = 1; i < 256; i++)
            {
                tempR[i] = tempR[i - 1] + precentR[i];
                tempG[i] = tempG[i - 1] + precentG[i];
                tempB[i] = tempB[i - 1] + precentB[i];
            }

            for (int i = 0; i < 256; i++)
            {
                nsgR[i] = (int)(tempR[i] * 255.0 + 0.5);
                nsgG[i] = (int)(tempG[i] * 255.0 + 0.5);
                nsgB[i] = (int)(tempB[i] * 255.0 + 0.5);
            }
        }

        private void improve()
        {
            wb = new WriteableBitmap(bmp);
            newImage = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight);
            int[] ImageData = wb.Pixels;
            for(int i=0;i<wb.PixelHeight;i++)
                for (int j = 0; j < wb.PixelWidth; j++)
                {
                    int pixel = ImageData[i * wb.PixelWidth + j];

                    //byte AValue = (byte)((pixel & 0xff) >> 24);
                    byte RedValue = (byte)((pixel & 0x00FF0000) >> 16);
                    byte GreenValue = (byte)((pixel & 0x0000FF00) >> 8);
                    byte BlueValue = (byte)(pixel & 0x000000FF);

                    int RedValue1 = nsgR[RedValue];
                    int GreenValue1 = nsgG[GreenValue];
                    int BlueValue1 = nsgB[BlueValue];

                    int Pixel = ((255 & 0xff) << 24) | ((RedValue1 & 0xff) << 16) | ((GreenValue1 & 0xff) << 8) | (BlueValue1 & 0xff);
                    newImage.Pixels[i * wb.PixelWidth + j] = Pixel;
                }
        }
    }
}