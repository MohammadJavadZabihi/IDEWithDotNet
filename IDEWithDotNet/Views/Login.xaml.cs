using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using ApiRequest.Net.CallApi;
using IDECore.DTOs;
using IDEWithDotNet.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

namespace IDEWithDotNet
{
    public sealed partial class MainWindow : Window
    {
        private CallApi _callApi;
        public MainWindow()
        {
            this.InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null);
            var appWindowPresent = this.AppWindow.Presenter as OverlappedPresenter;
            appWindowPresent.IsResizable = false;
            appWindowPresent.IsMaximizable = false;
            AppWindow.Resize(new SizeInt32(700, 700));

            _callApi = new CallApi();
        }

        private void goToRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            var registerPage = new RegisterPage();
            registerPage.Activate();
            this.Close();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                txtBorderError.Text = $"Error : Pleas Fill The All Entrys";
                errorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                bool remmebm = false;

                if (ckbRememberMe.IsChecked == true)
                {
                    remmebm = true;
                }

                var data = new
                {
                    Email = txtEmail.Text,
                    Password = txtPassword.Password,
                    UserName = txtEmail.Text,
                    RememberMe = remmebm
                };

                var responseMessage = await _callApi.SendPostRequest<LoginReturn>("https://localhost:7049/api/Users/Login", data);

                if (responseMessage.IsSuccess)
                {
                    MainText mainText = new MainText();
                    mainText.Activate();
                    this.Close();
                }
                else
                {
                    txtBorderError.Text = $"Error : {responseMessage.Message}";
                    errorBorder.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
