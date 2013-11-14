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

namespace NoFog
{
    public partial class Set : PhoneApplicationPage
    {
        public Set()
        {
            InitializeComponent();
            
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            App app;
            app = Application.Current as App;
            slider1.Value = app.t0;
            slider2.Value = app.w0;
            slider3.Value = app.lengh;
        }
        private void save_Click(object sender, EventArgs e)
        {
            App app;
            app = Application.Current as App;
            app.t0 = slider1.Value;
            app.w0 = slider2.Value;
            app.lengh = (int)slider3.Value;
            MessageBox.Show("设置已被保存");
        }

        private void reset_Click(object sender, EventArgs e)
        {
            slider1.Value = 0.15;
            slider2.Value = 0.65;
            slider3.Value = 5;
            MessageBox.Show("设置已经重制");
            save_Click(sender,e);
        }
    }
}