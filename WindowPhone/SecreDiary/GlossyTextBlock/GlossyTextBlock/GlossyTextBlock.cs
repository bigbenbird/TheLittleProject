using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GlossyTextBlock
{
    public class GlossyTextBlock : Control
    {
        public GlossyTextBlock()
        {
            // Apply default template
            DefaultStyleKey = typeof(GlossyTextBlock);
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("Text Property. DependencyProperty."), Category("SilverLaw GlossyTextblock")]
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(GlossyTextBlock),
            new PropertyMetadata("GlossyTextblock"));
    }
}
