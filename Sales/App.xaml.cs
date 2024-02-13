namespace Sales
{
    using System;
    using System.Threading.Tasks;
    using Common.Models;
    using Helpers;
    using Newtonsoft.Json;
    using Services;
    using ViewModels;
    using Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        #region Properties
        public static NavigationPage Navigator { get; internal set; }
        #endregion

        #region Constructors
        [System.Obsolete]
        public App()
        {
            InitializeComponent();

            if (Settings.IsRemembered)
            {
                if (!string.IsNullOrEmpty(Settings.UserASP))
                {
                    MainViewModel.GetInstance().UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);
                }

                MainViewModel.GetInstance().Products = new ProductsViewModel();
                this.MainPage = new MasterPage();
            }
            else
            {
                MainViewModel.GetInstance().Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion

        #region Methods
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Current.MainPage = new NavigationPage(new LoginPage()));
            }
        }

        [Obsolete]
        public static async Task NavigateToProfile(TokenResponse token)
        {
            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            Settings.IsRemembered = true;
            Settings.AccessToken = token.AccessToken;
            Settings.TokenType = token.TokenType;

            var apiService = new ApiService();
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await apiService.GetUser(url, prefix, $"{controller}/GetUser", token.UserName, token.TokenType, token.AccessToken);
            if (response.IsSuccess)
            {
                var userASP = (MyUserASP)response.Result;
                MainViewModel.GetInstance().UserASP = userASP;
                //MainViewModel.GetInstance().RegisterDevice();
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }

            //MainViewModel.GetInstance().Categories = new CategoriesViewModel();
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}
