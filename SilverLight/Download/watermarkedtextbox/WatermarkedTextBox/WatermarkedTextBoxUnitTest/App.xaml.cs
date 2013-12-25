///////////////////////////////////////////////////////////////////////////////
//
//  App.xaml
//
// 
// © 2008 Microsoft Corporation. All Rights Reserved.
//
// This file is licensed as part of the Silverlight 2 SDK, for details look here: 
// http://go.microsoft.com/fwlink/?LinkID=111970&clcid=0x409
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using Microsoft.Silverlight.Testing;

namespace WatermarkedTextBoxUnitTest
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Initialize and start the test framework
            this.RootVisual = (UIElement)UnitTestSystem.CreateTestPage(this);
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
    }
}
