﻿#pragma checksum "C:\Users\LiJianpeng\Documents\Visual Studio 2010\Projects\xidianjq\SecreDiary\XapNote\ViewEdit.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2856CD667AC1115EBA567C5D9FE98CA1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace XapNote {
    
    
    public partial class ViewEdit : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock displayTextBlock;
        
        internal System.Windows.Controls.TextBox editTextBox;
        
        internal System.Windows.Controls.Canvas confirmDialog;
        
        internal System.Windows.Controls.Button btnCancel;
        
        internal System.Windows.Controls.Button btnOk;
        
        internal System.Windows.Controls.Canvas passwordCanvs;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.PasswordBox passwordBox1;
        
        internal System.Windows.Controls.Button button1;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/XapNote;component/ViewEdit.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.displayTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("displayTextBlock")));
            this.editTextBox = ((System.Windows.Controls.TextBox)(this.FindName("editTextBox")));
            this.confirmDialog = ((System.Windows.Controls.Canvas)(this.FindName("confirmDialog")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
            this.btnOk = ((System.Windows.Controls.Button)(this.FindName("btnOk")));
            this.passwordCanvs = ((System.Windows.Controls.Canvas)(this.FindName("passwordCanvs")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.passwordBox1 = ((System.Windows.Controls.PasswordBox)(this.FindName("passwordBox1")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
        }
    }
}

