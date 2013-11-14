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
namespace XapNote
{
    /// <summary>
    /// 编辑日记界面，提供了编辑日记的入口
    /// </summary>
    public partial class ViewEdit : PhoneApplicationPage
    {

        private string fileName = "";
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;



        public ViewEdit()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(backKeyPress);
        }

        void backKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (passwordCanvs.Visibility == Visibility.Visible)
            {
                passwordCanvs.Visibility=Visibility.Collapsed;
                NavigationService.GoBack();
                return;
            }
            if (confirmDialog.Visibility == Visibility.Visible)
            {
                passwordCanvs.Visibility = Visibility.Collapsed;
                e.Cancel = true;
                return;
            }
            if (editTextBox.Visibility == Visibility.Visible)
            {
                editTextBox.Visibility = Visibility.Collapsed;
                e.Cancel = true;
                return;
            }
            if (confirmDialog.Visibility == Visibility.Visible)
            {
                confirmDialog.Visibility = Visibility.Collapsed;
                e.Cancel = true;
                return;
            }
        }
        private void Appbar_Back_Click(object sender, EventArgs e)
        {
            navigateBack();
        }

        private void Appbar_Edit_Click(object sender, EventArgs e)
        {
            if (this.displayTextBlock.Visibility == Visibility.Visible)
            {
                bindEdit(this.displayTextBlock.Text);
            }
        }

        private void Appbar_Save_Click(object sender, EventArgs e)
        {
            if (this.editTextBox.Visibility == Visibility.Visible)
            {
                var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

                try
                {
                    using (var fileStream = appStorage.OpenFile(fileName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        {
                            SMS sms = new SMS();
                            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                            int password;
                            settings.TryGetValue<int>("password", out password);
                            writer.WriteLine(sms.encode(this.editTextBox.Text,password));
                        }
                    }
                }
                catch (Exception)
                {

                }

                this.displayTextBlock.Text = this.editTextBox.Text;
                this.displayTextBlock.Visibility = Visibility.Visible;
                this.editTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void Appbar_Delete_Click(object sender, EventArgs e)
        {
            this.confirmDialog.Visibility = Visibility.Visible;
            // navigateBack();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string state = "";
            ApplicationBar.IsVisible = false;
            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "edit")
                    {
                        ApplicationBar.IsVisible = true;
                        string value = "";

                        if (settings.Contains("fileName"))
                        {
                            if (settings.TryGetValue<string>("fileName", out value))
                            {
                                fileName = value;
                            }
                        }

                        if (settings.Contains("value"))
                        {
                            if (settings.TryGetValue<string>("value", out value))
                            {
                                bindEdit(value);
                            }
                        }
                    }

                    else
                    {
                        bindView();
                    }
                }
            }

            else
            {
                bindView();
            }
        }


        private void bindView()
        {
            passwordCanvs.Visibility = Visibility.Visible;

        }



        private void bindEdit(string content)
        {
            this.editTextBox.Text = content;
            this.displayTextBlock.Visibility = Visibility.Collapsed;
            this.editTextBox.Visibility = Visibility.Visible;

            this.editTextBox.Focus();
            this.editTextBox.SelectionStart = this.editTextBox.Text.Length;

            settings["state"] = "edit";
            settings["value"] = this.editTextBox.Text;
            settings["fileName"] = fileName;
        }



        private void navigateBack()
        {
            settings["state"] = "";
            settings["value"] = "";
            settings["fileName"] = "";
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }




        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.confirmDialog.Visibility = Visibility.Collapsed;
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            if (fileName == "")
            {
                MessageBox.Show("你没有权限删除日记");
                return;
            }
            appStorage.DeleteFile(fileName);
            this.confirmDialog.Visibility = Visibility.Collapsed;
            navigateBack();
        }




        private void editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["value"] = this.editTextBox.Text;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(passwordBox1.Password))
            {
                MessageBox.Show("请输入数字密码");
                return;
            }
            if (passwordBox1.Password == "")
            {
                MessageBox.Show("请输入非空密码");
                return;
            }
            passwordCanvs.Visibility = Visibility.Collapsed;
            int password = int.Parse(passwordBox1.Password);
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            int oldPassword;
            settings.TryGetValue<int>("password", out oldPassword);
            if (password != oldPassword)
            {
                MessageBox.Show("密码错误！！将返回主页面");
                NavigationService.GoBack();
                return;
            }
            ApplicationBar.IsVisible = true;
            if (NavigationContext.QueryString["id"] != null)
            {
                fileName = NavigationContext.QueryString["id"];
            }

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            try
            {
                using (var file = appStorage.OpenFile(fileName, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        SMS sms = new SMS();
                        displayTextBlock.Text = sms.decode(reader.ReadToEnd(), password);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}