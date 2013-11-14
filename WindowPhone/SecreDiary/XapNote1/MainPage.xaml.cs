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
using System.IO.IsolatedStorage;
using System.IO;

namespace XapNote
{
    /// <summary>
    /// 程序的主界面，提供了日记操作的入口
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        #region 构造器

        public MainPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Appbar 事件

        #region 添加事件

        private void Appbar_Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/XapNote;component/Add.xaml", UriKind.Relative));

            #region 注销掉（测试用）
            /*
                           //0123456789012345678901234567890123456789   
            string fileName="2010_12_29_13_43_01_Woo_Gankyang-CHN.txt";
            string fileContent = "我的日记";

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!appStorage.FileExists(fileName))
            {
                using (var file = appStorage.CreateFile(fileName))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine(fileContent);
                    }
                }
            }

            bindList();
             * */

            #endregion
        }

        #endregion

        #region 帮助事件

        private void Appbar_Help_Click(object sender, EventArgs e)
        {
            this.helpCanvas.Visibility = Visibility.Visible;
        }

        #endregion

        #endregion

        #region 程序加载事件

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            string state = "";

            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "add")
                    {
                        NavigationService.Navigate(new Uri("/Add.xaml", UriKind.Relative));
                    }

                    else if (state == "edit")
                    {
                        NavigationService.Navigate(new Uri("/ViewEdit.xaml", UriKind.Relative));
                    }
                }
            }

            bindList();
        }

        #endregion

        #region ListBox绑定数据

        private void bindList()
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            List<Note> notes = new List<Note>();
            
            string[] fileList = appStorage.GetFileNames();

            foreach (var file in fileList)
            {
                if (file != "__ApplicationSettings")
                {
                    //2010_12_30_14_02_01_ddd.txt
                    string fileFullName = file;

                    string year = file.Substring(0, 4);
                    string month = file.Substring(5, 2);
                    string day = file.Substring(8, 2);
                    string hour = file.Substring(11, 2);
                    string minute = file.Substring(14, 2);
                    string second = file.Substring(17, 2);
                    DateTime dateTime = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));
                    string dateCreated = dateTime.ToString("yyyy年MM月dd日 HH:MM:ss");
                    string fileName = file.Substring(20);

                    fileName = fileName.Substring(0, fileName.Length - 4);

                    notes.Add(new Note() { FileFullName = fileFullName, DateCreated = dateCreated, FileName = fileName });
                }
            }

            noteListBox.ItemsSource = notes;
        }

        #endregion

        #region HyperlinkButton事件

        private void noteLocation_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton clickedLink = (HyperlinkButton)sender;
            string uri = string.Format("/XapNote;component/ViewEdit.xaml?id={0}", clickedLink.Tag);

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }

        #endregion

        #region 关闭帮助事件

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.helpCanvas.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}