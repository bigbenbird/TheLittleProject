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
namespace NoFog
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
        WriteableBitmap bmp;
        WriteableBitmap wb;
        App app;
        private byte[,] darkValue;
        private byte airDrak;
        private double t0 ;
        private double w ;
        int lengh;
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
            app = Application.Current as App;
            bmp = new WriteableBitmap(app.photoResult);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            
            //bmp.SetSource(app.photoResult.ChosenPhoto);
            
            image1.Source = bmp;
            t0 = app.t0;
            w = app.w0;
            lengh = app.lengh;
            Debug.WriteLine("t0:" + t0.ToString());
            Debug.WriteLine("w:" + w.ToString());
            Debug.WriteLine("lengh" + lengh.ToString());
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
                makeDarkChannel();
                //image1.Source = darkImage;
            }
            //progressBar1.Value = 50;
            if(i==70)
                removeFog();
            if (i == 100)
            {
                image1.Source = noFog;
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
        private void makeDarkChannel()
        {
            airDrak = 0;
            wb = new WriteableBitmap(bmp);
            darkValue = new byte[wb.PixelHeight, wb.PixelWidth];
            darkImage = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight);
            int[] ImageData = wb.Pixels;
            for (int i = 0; i < wb.PixelHeight-lengh; i+=lengh)
                for (int j = 0; j < wb.PixelWidth-lengh; j+=lengh)
                {
                    byte minValue = 255;
                    //在15×15的方块区域内寻找最低颜色通道
                    for (int m = 0; m < lengh && i+m<wb.PixelHeight; m++)
                        for (int n = 0; n < lengh && j+n<wb.PixelWidth; n++)
                        {
                            int pixel = ImageData[(i+m) * wb.PixelWidth + j+n];

                            //提取颜色
                            byte AValue = (byte)((pixel & 0xff) >> 24);
                            byte RedValue = (byte)((pixel & 0x00FF0000) >> 16);
                            byte GreenValue = (byte)((pixel & 0x0000FF00) >> 8);
                            byte BlueValue = (byte)(pixel & 0x000000FF);

                            minValue = Math.Min(minValue,RedValue);
                            minValue = Math.Min(minValue, GreenValue);
                            minValue = Math.Min(minValue, BlueValue);
                            #region
                            //int k = (255 << 24) | (194 << 16) | (194 << 8) | 206;
                            //Debug.WriteLine(k.ToString());
                            //if (i<15 && j < 15)
                            //{
                            //    Debug.WriteLine(minValue.ToString());
                            //    Debug.WriteLine(RedValue.ToString());
                            //    Debug.WriteLine(GreenValue.ToString());
                            //    Debug.WriteLine(BlueValue.ToString());
                            //    Debug.WriteLine(pixel.ToString());
                            //}
                            //if (i > wb.PixelHeight - 15 && j > wb.PixelWidth - 15)
                            //{
                            //    Debug.WriteLine(minValue.ToString());
                            //    Debug.WriteLine(RedValue.ToString());
                            //    Debug.WriteLine(GreenValue.ToString());
                            //    Debug.WriteLine(BlueValue.ToString());
                            //    Debug.WriteLine(pixel.ToString());
                            //}
                            #endregion
                            if (minValue > airDrak)
                            {
                                airDrak = minValue;
                                Debug.WriteLine("airDrak:"+airDrak.ToString());
                            }
                            #region
                            //int Pixel = ((255 & 0xff) << 24) | ((minValue & 0xff) << 16) | ((minValue & 0xff) << 8) | (minValue & 0xff);
                            //darkImage.Pixels[i * wb.PixelWidth + j] = Pixel;
                            #endregion

                        }
                    //把15×15方块里面的最小像素值存在darkValue数组中
                    for (int m = 0; m < lengh && i + m < wb.PixelHeight; m++)
                        for (int n = 0; n < lengh && j + n < wb.PixelWidth; n++)
                        {
                            darkValue[i + m, n + j] = minValue;

                        }
                    airDrak = airDrak > minValue ? airDrak : minValue;

                }
            //根据公式，算出每个位置上面的被折射像素
            darkChannelValue = new double[bmp.PixelHeight, bmp.PixelWidth];
            Debug.WriteLine("makeDarkChannel:"+lengh.ToString());
            //w = 0.65;
            for (int i = 0; i < bmp.PixelHeight; i++)
                for (int j = 0; j < bmp.PixelWidth; j++)
                {
                    darkChannelValue[i, j] = 1.0 - (w * (darkValue[i, j] / (0.0+airDrak)));
                    
                    
                }
        }
        private void removeFog() 
        {
            wb = new WriteableBitmap(bmp);
            noFog = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight);
            int[] ImageData = wb.Pixels;
            for(int i=0;i<bmp.PixelHeight;i++)
                for (int j = 0; j < bmp.PixelWidth; j++)
                {
                    int pixel = ImageData[i * wb.PixelWidth + j];
                    //提取像素值
                    byte RedValue = (byte)((pixel & 0x00FF0000) >> 16);
                    byte GreenValue = (byte)((pixel & 0x0000FF00) >> 8);
                    byte BlueValue = (byte)(pixel & 0x000000FF);
                    
                    //使用t0=0.1修正，避免darkChannelValue值过小
                    double t = Math.Max(darkChannelValue[i,j],t0);

                    //if (RedValue > airDrak || GreenValue > airDrak || BlueValue > airDrak)
                    //    Debug.WriteLine("原像素" + RedValue.ToString() + " " + GreenValue.ToString() + " " + BlueValue.ToString()+"t:"+t.ToString());
                    RedValue = (byte)((RedValue - airDrak) / t + airDrak);
                    GreenValue = (byte)((GreenValue - airDrak) / t + airDrak);
                    BlueValue = (byte)((BlueValue - airDrak) / t + airDrak);
                    #region
                    //RedValue = (byte)((RedValue - airDrak * (1.0 - darkChannelValue[i, j])) / darkChannelValue[i, j]);
                    //GreenValue = (byte)((GreenValue - airDrak * (1.0 - darkChannelValue[i,j])) / darkChannelValue[i,j]);
                    //BlueValue = (byte)((BlueValue - airDrak * (1.0 - darkChannelValue[i, j])) / darkChannelValue[i, j]);
                    #endregion

                    //if (RedValue > airDrak || GreenValue > airDrak || BlueValue > airDrak)
                    //    Debug.WriteLine("后像素" + RedValue.ToString() + " " + GreenValue.ToString() + " " + BlueValue.ToString());
                    int Pixel = ((255 & 0xff) << 24) | ((RedValue & 0xff) << 16) | ((GreenValue & 0xff) << 8) | (BlueValue & 0xff);

                    noFog.Pixels[i * wb.PixelWidth + j] = Pixel;
                }

        
        }
 
    }
}