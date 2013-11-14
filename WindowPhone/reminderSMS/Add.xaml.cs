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
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
namespace reminderSMS
{
    public partial class Add : PhoneApplicationPage
    {
        PhoneNumberChooserTask phoneChooser;
        IDictionary<string, object> appState = PhoneApplicationService.Current.State;
        public Add()
        {
            InitializeComponent();
            phoneChooser = new PhoneNumberChooserTask();
            phoneChooser.Completed += new EventHandler<PhoneNumberResult>(phoneChooser_Completed);
        }
        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {
            // The code in the following steps goes here.
            // Generate a unique name for the new notification. You can choose a
            // name that is meaningful for your app, or just use a GUID.
            String name = System.Guid.NewGuid().ToString();
            // Get the begin time for the notification by combining the DatePicker
            // value and the TimePicker value.
            DateTime date = (DateTime)beginDatePicker.Value;
            DateTime time = (DateTime)beginTimePicker.Value;
            DateTime beginTime = date + time.TimeOfDay;

            // Make sure that the begin time has not already passed.
            if (beginTime < DateTime.Now)
            {
                MessageBox.Show("初始时间必须应该在未来");
                return;
            }

            // Determine which recurrence radio button is checked.
            RecurrenceInterval recurrence = RecurrenceInterval.None;

            // Create a URI for the page that will be launched if the user
            // taps on the reminder. Use query string parameters to pass 
            // content to the page that is launched.
            string queryString = "";
            if (textBlock1.Text != "")
                queryString += ("?phone1=" + textBlock1.Text);
            if (textBlock2.Text != "")
                queryString += ("&phone2=" + textBlock2.Text);
            if (textBlock3.Text != "")
                queryString += ("&phone3=" + textBlock3.Text);
            //queryString += ("&text=" + contentTextBox.Text);
            queryString += ("&name=" + name);
            Uri navigationUri = new Uri("/sendSMS.xaml" + queryString, UriKind.Relative);

            {
                Reminder reminder = new Reminder(name);
                reminder.Title = titleTextBox.Text;
                reminder.Content = contentTextBox.Text;
                reminder.BeginTime = beginTime;
                reminder.ExpirationTime = beginTime.AddMinutes(5) ;
                reminder.RecurrenceType = recurrence;
                reminder.NavigationUri = navigationUri;

                // Register the reminder with the system.
                ScheduledActionService.Add(reminder);
            }
            // Navigate back to the main reminder list page.
            NavigationService.GoBack();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            appState["ClickedButtonID"] = ((Button)sender).Name;
            appState["TxbSelectPhone1"] = textBlock1.Text;
            appState["TxbSelectPhone2"] = textBlock2.Text;
            appState["TxbSelectPhone3"] = textBlock3.Text;

            phoneChooser.Show();
        }
        void phoneChooser_Completed(object sender, PhoneNumberResult e)
        {
            textBlock1.Text = appState["TxbSelectPhone1"].ToString();
            textBlock2.Text = appState["TxbSelectPhone2"].ToString();
            textBlock3.Text = appState["TxbSelectPhone3"].ToString();

            if (e.TaskResult == TaskResult.OK)
            {
                switch (appState["ClickedButtonID"].ToString())
                {
                    case "button1":
                        textBlock1.Text = e.PhoneNumber;
                        break;
                    case "button2":
                        textBlock2.Text = e.PhoneNumber;
                        break;
                    case "button3":
                        textBlock3.Text = e.PhoneNumber;
                        break;
                }
            }

        }

    }
}