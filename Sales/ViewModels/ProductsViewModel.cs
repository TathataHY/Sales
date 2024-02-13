namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private DataService dataService;
        private ObservableCollection<ProductItemViewModel> products;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<ProductItemViewModel> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }
        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }
        public string Filter
        {
            get => this.filter;
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }
        public List<Product> MyProducts { get; set; }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            instance = this;

            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (connection.IsSuccess)
            {
                var answer = await this.LoadProductsFromAPI();
                if (answer)
                {
                    _ = this.SaveProductsToDB();
                }
            }
            else
            {
                await this.LoadProductsFromDB();

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
            }

            if (this.MyProducts == null || this.MyProducts.Count == 0)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NoProductsMessage,
                    Languages.Accept);
                return;
            }

            this.RefreshList();

            this.IsRefreshing = false;
        }
        private async Task LoadProductsFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }
        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.GetList<Product>(
                url,
                prefix,
                controller,
                Settings.TokenType,
                Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
        }
        private async Task SaveProductsToDB()
        {
            await this.dataService.DeleteAllProduct();
            _ = this.dataService.Insert(this.MyProducts);
        }
        public void RefreshList()
        {
            IEnumerable<ProductItemViewModel> list = null;

            if (string.IsNullOrWhiteSpace(this.Filter))
            {
                list = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                });
            }
            else
            {
                list = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower()));
            }

            this.Products = new ObservableCollection<ProductItemViewModel>(list.OrderBy(p => p.Description));
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand => new RelayCommand(LoadProducts);
        public ICommand SearchCommand => new RelayCommand(RefreshList);
        #endregion
    }
}
