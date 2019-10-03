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
using Microsoft.Phone.Internals;

namespace WP7SimulatorAppTemplate
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WinPhoneBoard board = new WinPhoneBoard();
            this.RootVisual = board;
        }

        private void Application_Exit(object sender, EventArgs e)
        {
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
            e.Handled = true;
        }
    }
}