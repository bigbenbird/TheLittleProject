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
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Phone.Tasks;

namespace SMSecret
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*$");
            string password = passwordBox1.Password;
            if (!regex.IsMatch(password) || string.IsNullOrEmpty(password)==true)
            {
                MessageBox.Show("请输入数字密码");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text) == true)
            {
                MessageBox.Show("请输入加密内容");
                return;
            }
            SMS code = new SMS();
            
            int pw = password[0]-'0';
            for (int i = 1; i < password.Length; i++)
            {
                pw *= 10;
                pw += password[i] - '0';
            }
            string encodeStr = code.encode(textBox1.Text,pw);
            textBox1.Text = encodeStr;
            if (radioButton1.IsChecked == true)
            {
                SmsComposeTask task = new SmsComposeTask();
                task.Body = encodeStr;
                task.Show();
            }
            else
            {
                if (radioButton2.IsChecked == true)
                {
                    EmailComposeTask email = new EmailComposeTask();
                    email.Body = encodeStr;
                    email.Show();
                }
                else
                {
                    MessageBox.Show("请输入发送方式");
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            string password = passwordBox1.Password;
            Regex regex = new Regex("^[0-9]*$");
            if (!regex.IsMatch(passwordBox1.Password) || string.IsNullOrEmpty(password) == true)
            {
                MessageBox.Show("请输入数字密码");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text) == true)
            {
                MessageBox.Show("请输入解密内容");
                return;
            }
            SMS code = new SMS();
            int pw = password[0] - '0';
            for (int i = 1; i < password.Length; i++)
            {
                pw *= 10;
                pw += password[i] - '0';
            }
            SMS test = new SMS();
            try
            {
                textBox1.Text = test.decode(textBox1.Text, pw);
            }
            catch
            {
                MessageBox.Show("密文无效，请输入正确密文");
            
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
    }
}