namespace Sales.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using Views;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructors
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private async void Edit()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await App.Navigator.PushAsync(new EditProductPage());
        }
        private async void Delete()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await apiService.Delete(
                url,
                prefix,
                controller,
                this.ProductId,
                Settings.TokenType,
                Settings.AccessToken);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var viewmodel = ProductsViewModel.GetInstance();
            var deletedproduct = viewmodel.MyProducts.FirstOrDefault(
                p => p.ProductId == this.ProductId);

            if (deletedproduct != null)
            {
                viewmodel.MyProducts.Remove(deletedproduct);
                viewmodel.RefreshList();
            }
        }
        #endregion

        #region Commands
        public ICommand EditCommand => new RelayCommand(Edit);
        public ICommand DeleteCommand => new RelayCommand(Delete);
        #endregion
    }
}
