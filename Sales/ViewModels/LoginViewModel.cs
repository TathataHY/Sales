namespace Sales.ViewModels
{
    using System;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Newtonsoft.Json;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.IsRemembered = true;
        }
        #endregion

        #region Methods
        private void ForgotPassword()
        {
            throw new NotImplementedException();
        }
        [Obsolete]
        private async void Login()
        {
            this.IsEnabled = false;
            this.IsRunning = true;

            if (string.IsNullOrWhiteSpace(this.Email))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.EmailValidation,
                   Languages.Accept);
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Password))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                   Languages.Error,
                   Languages.PasswordValidation,
                   Languages.Accept);
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await apiService.GetToken(
                    url,
                    this.Email,
                    this.Password);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept);
                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.IsRemembered = this.IsRemembered;

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await this.apiService.GetUser(
                url,
                prefix,
                $"{controller}/GetUser",
                this.Email,
                token.TokenType,
                token.AccessToken);

            if (response.IsSuccess)
            {
                var userASP = (MyUserASP)response.Result;
                MainViewModel.GetInstance().UserASP = userASP;
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }

            MainViewModel.GetInstance().Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();

            this.IsEnabled = true;
            this.IsRunning = false;
        }
        private async void Register()
        {
            this.IsEnabled = false;

            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());

            this.IsEnabled = true;
        }
        private async void LoginFacebook()
        {
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new LoginFacebookPage());

            this.IsEnabled = true;
        }
        private async void LoginTwitter()
        {
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new LoginTwitterPage());

            this.IsEnabled = true;
        }
        private async void LoginInstagram()
        {
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(new LoginInstagramPage());

            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ForgotPasswordCommand => new RelayCommand(ForgotPassword);
        [Obsolete]
        public ICommand LoginCommand => new RelayCommand(Login);
        public ICommand RegisterCommand => new RelayCommand(Register);
        public ICommand LoginFacebookCommand => new RelayCommand(LoginFacebook);
        public ICommand LoginTwitterCommand => new RelayCommand(LoginTwitter);
        public ICommand LoginInstagramCommand => new RelayCommand(LoginInstagram);
        #endregion
    }
}
