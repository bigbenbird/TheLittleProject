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

namespace SMSOS
{
    public partial class SetEvent : PhoneApplicationPage
    {
        public SetEvent()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                textBox1.Text = NavigationContext.QueryString["1"];
                textBox2.Text = NavigationContext.QueryString["2"];
                textBox3.Text = NavigationContext.QueryString["3"];
                textBox4.Text = NavigationContext.QueryString["4"];
                textBox5.Text = NavigationContext.QueryString["5"];
                textBox6.Text = NavigationContext.QueryString["6"];
                textBox7.Text = NavigationContext.QueryString["7"];
                textBox8.Text = NavigationContext.QueryString["8"];
                textBox9.Text = NavigationContext.QueryString["9"];
            }
            catch
            {
                //throw;
            }

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string s = string.Format("/MainPage.xaml?from=setEvent&1={0}&2={1}&3={2}&4={3}&5={4}&6={5}&7={6}&8={7}&9={8}",
                textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);
            NavigationService.Navigate(new Uri(s,UriKind.Relative));
        }
    }
}