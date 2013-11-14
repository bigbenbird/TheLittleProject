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
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace XapNote
{
    /// <summary>
    /// 程序的主界面，提供了日记操作的入口
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {

        public MainPage()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains("password"))
            {
                first.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = false;
            }
        }




        private void Appbar_Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Add.xaml", UriKind.Relative));

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



        private void Appbar_Help_Click(object sender, EventArgs e)
        {
            this.helpCanvas.Visibility = Visibility.Visible;
        }
        private void Appbar_Set_Click(object sender, EventArgs e)
        {
            this.password.Visibility = Visibility.Visible;

        }



        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            bindList();
        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (helpCanvas.Visibility == Visibility.Visible)
            {
                helpCanvas.Visibility = Visibility.Collapsed;
                e.Cancel = true;
                return;
            }
            if (password.Visibility == Visibility.Visible)
            {
                password.Visibility = Visibility.Collapsed;
                e.Cancel = true;
                Debug.WriteLine("password cancel");
                return;
            }
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
            //NavigationService.GoBack();
            MessageBoxResult msgRst = MessageBox.Show("要退出本程序吗？", "提示", MessageBoxButton.OKCancel);
            if (msgRst == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            //NavigationService.GoBack();
            
        }

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



        private void noteLocation_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton clickedLink = (HyperlinkButton)sender;
            string uri = string.Format("/ViewEdit.xaml?id={0}", clickedLink.Tag);

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.helpCanvas.Visibility = Visibility.Collapsed;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.password.Visibility = Visibility.Collapsed;
        }

        private void sure_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*$");
            if (passwordBox1.Password != "" && passwordBox2.Password != "" && passwordBox3.Password != "")
            {
                if (regex.IsMatch(passwordBox1.Password) && regex.IsMatch(passwordBox2.Password) && regex.IsMatch(passwordBox3.Password))
                {
                    IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                    int oldPassword;
                    settings.TryGetValue<int>("password", out oldPassword);
                    if (oldPassword != int.Parse(passwordBox1.Password))
                    {
                        MessageBox.Show("原密码错误");
                    }
                    else if (passwordBox2.Password != passwordBox3.Password)
                        MessageBox.Show("两次密码不同");
                    else
                    {
                        settings["password"] = int.Parse(passwordBox3.Password);
                        int newPassword = int.Parse(passwordBox3.Password);


                        var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

                        string[] fileList = appStorage.GetFileNames();

                        foreach (var file in fileList)
                        {
                            string newtext;
                            using (var filename = appStorage.OpenFile(file, FileMode.Open))
                            {
                                using (StreamReader reader = new StreamReader(filename))
                                {
                                    string text = reader.ReadToEnd();
                                    SMS sms = new SMS();
                                    string text1 = sms.decode(text, oldPassword);
                                    Debug.WriteLine(text1);
                                    newtext = sms.encode(text1, newPassword);
                                }
                            }
                            using (var filestream = appStorage.OpenFile(file, FileMode.Create))
                            {
                                using (StreamWriter writer = new StreamWriter(filestream))
                                {
                                    writer.WriteLine(newtext);
                                }
                            }
                        }
                        settings.Save();
                        password.Visibility = Visibility.Collapsed;
                    }

                }
                else
                    MessageBox.Show("请输入数字密码");
            }
            else
                MessageBox.Show("密码不能为空");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*$");
            if (regex.IsMatch(textBox4.Text))
            {
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                if (textBox4.Text != "")
                {
                    int password = int.Parse(textBox4.Text);
                    settings.Add("password", password);
                    settings.Save();
                    first.Visibility = Visibility.Collapsed;
                    ApplicationBar.IsVisible = true;
                }
                else
                    MessageBox.Show("请输入非空密码");
            }
            else
                MessageBox.Show("请输入数字密码");
        }

        private void Appbar_About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

    }
}