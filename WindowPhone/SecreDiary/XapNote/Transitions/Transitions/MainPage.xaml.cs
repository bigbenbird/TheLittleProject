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
using System.ComponentModel;

namespace Transitions
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            //If no item is selected, do nothing
            if (listBox.SelectedItem == null) return;

            //Get the current selected MainTile
            NavigationTransition selectedTransition = listBox.SelectedItem as NavigationTransition;

            //Navigate to the selected Tile's Uri
            if (selectedTransition.NavigationUri != null) NavigationService.Navigate(selectedTransition.NavigationUri);

            //Reset the selected item to null
            listBox.SelectedItem = null;
        }

       
    }
    public class NavigationTransition
    {
        public string Title { get; set; }

        [TypeConverter(typeof(UriTypeConverter))]
        public Uri NavigationUri { get; set; }
    }
}