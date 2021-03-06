﻿using System;
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
using System.Text;
using System.IO;

namespace XapNote
{
    /// <summary>
    /// 添加日记界面，提供了添加日记的入口
    /// </summary>
    public partial class Add : PhoneApplicationPage
    {
        #region 私有变量
        private string fileFullName;
        private string fileName;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        #endregion

        #region 构造器

        public Add()
        {
            InitializeComponent();
        }

        #endregion

        #region Appbar事件

        #region 保存日记事件

        private void Appbar_Save_Click(object sender, EventArgs e)
        {
            this.displayTextBlock.Text = this.editTextBox.Text;
            this.displayTextBlock.Visibility = Visibility.Visible;
            this.editTextBox.Visibility = Visibility.Collapsed;
            this.namedDialog.Visibility = Visibility.Visible;
        }

        #endregion

        #region 取消保存日记事件

        private void Appbar_Cancel_Click(object sender, EventArgs e)
        {
            navigateBack();
        }

        #endregion

        #endregion

        #region 页面导航方法

        private void navigateBack()
        {
            settings["state"] = "";
            settings["value"] = "";
            NavigationService.Navigate(new Uri("/XapNote;component/MainPage.xaml", UriKind.Relative));
        }

        #endregion

        #region 程序加载事件

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string state = "";

            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "add")
                    {

                        string value = "";
                        if (settings.Contains("value"))
                        {
                            if (settings.TryGetValue<string>("value", out value))
                            {
                                this.editTextBox.Text = value;
                            }
                        }
                    }
                }
            }

            settings["state"] = "add";
            settings["value"] = this.editTextBox.Text;
            this.editTextBox.SelectionStart = this.editTextBox.Text.Length;
            this.editTextBox.Focus();
        }

        #endregion

        #region editTextBox_TextChanged

        private void editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["value"] = this.editTextBox.Text;
        }

        #endregion

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.fileNameTextBox.Text = "";
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.namedDialog.Visibility = Visibility.Collapsed;
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year);
            sb.Append("_");
            sb.Append(string.Format("{0:00}", DateTime.Now.Month));
            sb.Append("_");
            sb.Append(string.Format("{0:00}", DateTime.Now.Day));
            sb.Append("_");
            sb.Append(string.Format("{0:00}", DateTime.Now.Hour));
            sb.Append("_");
            sb.Append(string.Format("{0:00}", DateTime.Now.Minute));
            sb.Append("_");
            sb.Append(string.Format("{0:00}", DateTime.Now.Second));
            sb.Append("_");
            sb.Append(fileName);
            sb.Append(".txt");

            fileFullName = sb.ToString();

            try
            {
                using (var fileStream = appStorage.OpenFile(fileFullName, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.WriteLine(this.editTextBox.Text);
                    }
                }
            }
            catch (Exception)
            {

            }

            navigateBack();
        }

        private void fileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.fileName = this.fileNameTextBox.Text;
        }
    }
}