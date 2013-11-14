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
    /// 编辑日记界面，提供了编辑日记的入口
    /// </summary>
    public partial class ViewEdit : PhoneApplicationPage
    {
        #region 私有变量

        private string fileName = "";
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        #endregion

        #region 构造器

        public ViewEdit()
        {
            InitializeComponent();
        }

        #endregion

        #region Appbar事件

        #region 返回主界面事件

        private void Appbar_Back_Click(object sender, EventArgs e)
        {
            navigateBack();
        }

        #endregion

        #region 编辑日记事件

        private void Appbar_Edit_Click(object sender, EventArgs e)
        {
            if (this.displayTextBlock.Visibility == Visibility.Visible)
            {
                bindEdit(this.displayTextBlock.Text);
            }
        }

        #endregion

        #region 保存日记事件

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
                            writer.WriteLine(this.editTextBox.Text);
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

        #endregion

        #region 删除日记事件

        private void Appbar_Delete_Click(object sender, EventArgs e)
        {
            this.confirmDialog.Visibility = Visibility.Visible;
            // navigateBack();
        }

        #endregion

        #endregion

        #region 页面加载事件

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string state = "";

            if (settings.Contains("state"))
            {
                if (settings.TryGetValue<string>("state", out state))
                {
                    if (state == "edit")
                    {
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

        #endregion

        #region bindView

        private void bindView()
        {
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
                        displayTextBlock.Text = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {

            }

        }

         #endregion

        #region bindEdit

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

        #endregion

        #region 页面导航事件

        private void navigateBack()
        {
            settings["state"] = "";
            settings["value"] = "";
            settings["fileName"] = "";
            NavigationService.Navigate(new Uri("/XapNote;component/MainPage.xaml", UriKind.Relative));
        }

        #endregion

        #region 取消删除事件

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.confirmDialog.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region 确定删除事件

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            appStorage.DeleteFile(fileName);
            this.confirmDialog.Visibility = Visibility.Collapsed;
            navigateBack();
        }

        #endregion

        #region editTextBox_TextChanged

        private void editTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            settings["value"] = this.editTextBox.Text;
        }

        #endregion
    }
}