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
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace SMSOS
{
    public partial class SetFriends : PhoneApplicationPage
    {
        const string ClickedButtonID = "ClickedButtonID";
        const string TxbSelectPhone1 = "TxbSelectPhone1";
        const string TxbSelectPhone2 = "TxbSelectPhone2";
        const string TxbSelectPhone3 = "TxbSelectPhone3";
        const string FriendsPhoneList = "FriendsPhoneList";

        
        PhoneNumberChooserTask phoneChooser;
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        IDictionary<string, object> appState = PhoneApplicationService.Current.State;

        public SetFriends()
        {
            InitializeComponent();

            phoneChooser = new PhoneNumberChooserTask();
            phoneChooser.Completed += new EventHandler<PhoneNumberResult>(phoneChooser_Completed);

            //从独立存储中加载预设好友列表
            if (settings.Contains(FriendsPhoneList))
            {
                string[] phones = (string[])settings[FriendsPhoneList];
                txbPhones1.Text = phones[0];
                txbPhones2.Text = phones[1];
                txbPhones3.Text = phones[2];
            }
        }


        //选择亲友号码
        private void btnSelectPhone_Click(object sender, RoutedEventArgs e)
        {
            appState[ClickedButtonID] = ((Button)sender).Name;
            appState[TxbSelectPhone1] = txbPhones1.Text;
            appState[TxbSelectPhone2] = txbPhones2.Text;
            appState[TxbSelectPhone3] = txbPhones3.Text;

            phoneChooser.Show();
        }

        //选择亲友号码完成
        void phoneChooser_Completed(object sender, PhoneNumberResult e)
        {
            txbPhones1.Text = appState[TxbSelectPhone1].ToString();
            txbPhones2.Text = appState[TxbSelectPhone2].ToString();
            txbPhones3.Text = appState[TxbSelectPhone3].ToString();
            
            if(e.TaskResult==TaskResult.OK)
            {
                switch (appState[ClickedButtonID].ToString())
                {
                    case "btnSelectPhone1":
                        txbPhones1.Text=e.PhoneNumber;
                        break;
                    case "btnSelectPhone2":
                        txbPhones2.Text=e.PhoneNumber;
                        break;
                    case "btnSelectPhone3":
                        txbPhones3.Text=e.PhoneNumber;
                        break;
                }
            }

        }

        //保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string[] phones = new string[]
            {
                txbPhones1.Text.Trim(),
                txbPhones2.Text.Trim(),
                txbPhones3.Text.Trim()
            };            
            settings[FriendsPhoneList] = phones;
            settings.Save();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        //取消
        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

       

    }
}