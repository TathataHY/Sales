namespace Sales.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Methods     
        private void GoTo()
        {
            if (this.PageName == "LoginPage")
            {
                Settings.TokenType = string.Empty;
                Settings.AccessToken = string.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion

        #region Commands
        public ICommand GoToCommand => new RelayCommand(GoTo);
        #endregion
    }
}
