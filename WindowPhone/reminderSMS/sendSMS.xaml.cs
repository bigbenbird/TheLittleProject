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

using Microsoft.Phone.Scheduler;
namespace reminderSMS
{
    public partial class sendSMS : PhoneApplicationPage
    {
        string name="";
        public sendSMS()
        {
            InitializeComponent();
        }
        // Implement the OnNavigatedTo method and use NavigationContext.QueryString
        // to get the parameter values passed by the reminder.
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string phone1 = "";
            string phone2= "";
            string phone3 = "";
            string name1 = "";

            NavigationContext.QueryString.TryGetValue("phone1", out phone1);
            NavigationContext.QueryString.TryGetValue("phone2", out phone2);
            NavigationContext.QueryString.TryGetValue("phone3", out phone3);
            NavigationContext.QueryString.TryGetValue("name", out name1);

            if (name1 != name)
            {
                ScheduledNotification sms;
                sms = (ScheduledNotification)ScheduledActionService.Find(name1);
                SmsComposeTask smsCompose = new SmsComposeTask();
                smsCompose.Body = sms.Content;
                smsCompose.To = phone1 + "," + phone2 + "," + phone3;
                smsCompose.Show();
                name = name1;
            }
        }

    }
}