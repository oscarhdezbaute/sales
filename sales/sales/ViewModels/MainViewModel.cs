namespace sales.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using sales.Common.Models;
    using sales.Helpers;
    using sales.Views;    

    public class MainViewModel
    {
        #region Properties
        public ProductsViewModel Products { get; set; }
        public CategoriesViewModel Categories { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public LoginViewModel Login { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public RegisterViewModel Register { get; set; }
        public MyUserASP UserASP { get; set; }
        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }
        public string UserImageFullPath
        {
            get
            {
                foreach (var claim in this.UserASP.Claims)
                {
                    if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
                    {
                        if (claim.ClaimValue.StartsWith("~"))
                        {
                            return $"http://172.24.0.1/Sales.API{claim.ClaimValue.Substring(1)}";
                        }

                        return claim.ClaimValue;
                    }
                }

                return null;
            }

        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
        }

        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion

        #region Commands
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }
        #endregion

        #region Methods
        private async void GoToAddProduct()
        {            
            this.AddProduct = new AddProductViewModel(this.Products.Category);
            await App.Navigator.PushAsync(new AddProductPage());
        }
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_map",
                PageName = "MapPage",
                Title = Languages.Map,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });

        }
        #endregion
    }
}
