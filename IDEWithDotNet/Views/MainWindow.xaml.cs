using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using IDEWithDotNet.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

namespace IDEWithDotNet
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null);
            var appWindowPresent = this.AppWindow.Presenter as OverlappedPresenter;
            appWindowPresent.IsResizable = false;
            appWindowPresent.IsMaximizable = false;
            AppWindow.Resize(new SizeInt32(700, 700));
        }

        private void goToRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            var registerPage = new RegisterPage();
            registerPage.Activate();
            this.Close();
        }
    }
}
