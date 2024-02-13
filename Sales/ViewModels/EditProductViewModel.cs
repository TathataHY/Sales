namespace Sales.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;
        private Product product;
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
        public ImageSource ImageSource
        {
            get => this.imageSource;
            set => this.SetValue(ref this.imageSource, value);
        }
        public Product Product
        {
            get => this.product;
            set => this.SetValue(ref this.product, value);
        }
        #endregion

        #region Constructors
        public EditProductViewModel(Product product)
        {
            this.Product = product;
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = product.ImageFullPath;
        }
        #endregion

        #region Methods
        private async void Edit()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            if (string.IsNullOrEmpty(this.Product.Description))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }

            if (this.Product.Price < 0)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await apiService.Put(
                url,
                prefix,
                controller,
                this.Product,
                this.Product.ProductId,
                Settings.TokenType,
                Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var newproduct = (Product)response.Result;
            var viewmodel = ProductsViewModel.GetInstance();

            var oldproduct = viewmodel.MyProducts.FirstOrDefault(p => p.ProductId == this.Product.ProductId);
            if (oldproduct != null)
            {
                viewmodel.MyProducts.Remove(newproduct);
            }

            viewmodel.MyProducts.Add(newproduct);
            viewmodel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;

            await App.Navigator.PopAsync();
        }
        private async void Delete()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
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
                this.Product.ProductId,
                Settings.TokenType,
                Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var viewmodel = ProductsViewModel.GetInstance();
            var deletedproduct = viewmodel.MyProducts.FirstOrDefault(
                p => p.ProductId == this.Product.ProductId);

            if (deletedproduct != null)
            {
                viewmodel.MyProducts.Remove(deletedproduct);
                viewmodel.RefreshList();
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            await App.Navigator.PopAsync();
        }
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
        #endregion

        #region Commands
        public ICommand EditCommand => new RelayCommand(Edit);
        public ICommand DeleteCommand => new RelayCommand(Delete);
        public ICommand ChangeImageCommand => new RelayCommand(ChangeImage);
        #endregion
    }
}
