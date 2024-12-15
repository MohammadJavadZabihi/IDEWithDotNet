using ApiRequest.Net.CallApi;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using Windows.UI;

namespace IDEWithDotNet.Views
{
    public sealed partial class RegisterPage : Window
    {
        private CallApi _callApi;

        public RegisterPage()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(null);
            var appWindowPresent = this.AppWindow.Presenter as OverlappedPresenter;
            appWindowPresent.IsResizable = false;
            appWindowPresent.IsMaximizable = false;
            AppWindow.Resize(new SizeInt32(700,700));

            _callApi = new CallApi();
        }

        private void goToLoginPage_Click(object sender, RoutedEventArgs e)
        {
            var loginPage = new MainWindow();
            loginPage.Activate();
            this.Close();
        }

        private async void btnResgiter_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtUserName.Text) || 
                string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtRePassword.Password))
            {
                txtBorderError.Text = $"Error : Pleas Fill The All Entrys";
                errorBorder.Visibility = Visibility.Visible;
            }

            var data = new
            {
                UserName = txtUserName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Password,
                Repassword = txtRePassword.Password
            };

            var responseMessgae = await _callApi.SendPostRequest<bool>("https://localhost:7049/api/Users/Register", data);

            if(responseMessgae.IsSuccess)
            {
                txtBorderError.Text = "Successfully Resgisterin, Pleas Go and Confrim your Email";
                errorBorder.Background = new SolidColorBrush(Color.FromArgb(255, 17, 139, 80));
                txtBorderError.FontSize = 14;
                errorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                txtBorderError.Text = $"Error : {responseMessgae.Message}";
                errorBorder.Visibility = Visibility.Visible;
            }
        }
    }
}
