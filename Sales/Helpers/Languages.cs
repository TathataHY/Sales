namespace Sales.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;
        public static string Error => Resource.Error;
        public static string NoInternet => Resource.NoInternet;
        public static string TurnOnInternet => Resource.TurnOnInternet;
        public static string Products => Resource.Products;
        public static string AddProduct => Resource.AddProduct;
        public static string Description => Resource.Description;
        public static string DescriptionPlaceholder => Resource.DescriptionPlaceholder;
        public static string Price => Resource.Price;
        public static string PricePlaceholder => Resource.PricePlaceholder;
        public static string Remarks => Resource.Remarks;
        public static string RemarksPlaceholder => Resource.RemarksPlaceholder;
        public static string Save => Resource.Save;
        public static string ChangeImage => Resource.ChangeImage;
        public static string DescriptionError => Resource.DescriptionError;
        public static string PriceError => Resource.PriceError;
        public static string ImageSource => Resource.ImageSource;
        public static string FromGallery => Resource.FromGallery;
        public static string NewPicture => Resource.NewPicture;
        public static string Cancel => Resource.Cancel;
        public static string Delete => Resource.Delete;
        public static string Edit => Resource.Edit;
        public static string DeleteConfirmation => Resource.DeleteConfirmation;
        public static string Yes => Resource.Yes;
        public static string No => Resource.No;
        public static string Confirm => Resource.Confirm;
        public static string EditProduct => Resource.EditProduct;
        public static string IsAvailable => Resource.IsAvailable;
        public static string Search => Resource.Search;
        public static string Login => Resource.Login;
        public static string Email => Resource.Email;
        public static string EmailPlaceholder => Resource.EmailPlaceholder;
        public static string Password => Resource.Password;
        public static string PasswordPlaceholder => Resource.PasswordPlaceholder;
        public static string RememberMe => Resource.RememberMe;
        public static string Forgot => Resource.Forgot;
        public static string Register => Resource.Register;
        public static string EmailValidation => Resource.EmailValidation;
        public static string PasswordValidation => Resource.PasswordValidation;
        public static string SomethingWrong => Resource.SomethingWrong;
        public static string Menu => Resource.Menu;
        public static string About => Resource.About;
        public static string Setup => Resource.Setup;
        public static string Exit => Resource.Exit;
        public static string NoProductsMessage => Resource.NoProductsMessage;
        public static string FirstName => Resource.FirstName;
        public static string LastName => Resource.LastName;
        public static string Phone => Resource.Phone;
        public static string Address => Resource.Address;
        public static string FirstNamePlaceholder => Resource.FirstNamePlaceholder;
        public static string LastNamePlaceholder => Resource.LastNamePlaceholder;
        public static string PhonePlaceholder => Resource.PhonePlaceholder;
        public static string AddressPlaceholder => Resource.AddressPlaceholder;
        public static string PasswordConfirm => Resource.PasswordConfirm;
        public static string PasswordConfirmPlaceholder => Resource.PasswordConfirmPlaceholder;
        public static string FirstNameError => Resource.FirstNameError;
        public static string LastNameError => Resource.LastNameError;
        public static string EmailError => Resource.EmailError;
        public static string PhoneError => Resource.PhoneError;
        public static string PasswordError => Resource.PasswordError;
        public static string PasswordConfirmError => Resource.PasswordConfirmError;
        public static string PasswordsNoMatch => Resource.PasswordsNoMatch;
        public static string RegisterConfirmation => Resource.RegisterConfirmation;
        public static string Categories => Resource.Categories;
        public static string Category => Resource.Category;
        public static string CategoryPlaceholder => Resource.CategoryPlaceholder;
        public static string CategoryError => Resource.CategoryError;
    }
}
