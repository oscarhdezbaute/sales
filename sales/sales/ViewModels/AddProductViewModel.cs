namespace sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using sales.Common.Models;
    using sales.Helpers;
    using sales.Services;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ApiService apiService;
        private ImageSource imageSource;
        private MediaFile file;
        private ObservableCollection<Category> categories;
        private Category category;
        #endregion

        #region Properties
        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        public List<Category> MyCategories { get; set; }
        public Category Category
        {
            get { return this.category; }
            set { this.SetValue(ref this.category, value); }
        }
        public ObservableCollection<Category> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }        
        #endregion

        #region Constructors
        public AddProductViewModel(Category category)
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = "noproduct";
            this.LoadCategories(category);            
        }        
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        } 
        #endregion

        #region Methods
        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }
            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }
            if (this.Category == null)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.CategoryError, Languages.Accept);
                return;
            }


            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection(false);
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            //verifico que el usuario haya seleccionado una foto, ya sea de la galeria o la tiró desde la camara
            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var product = new Product
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
                ImageArray = imageArray,
                CategoryId = this.Category.CategoryId,
                UserId = MainViewModel.GetInstance().UserASP.Id,
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller, product, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var newProduct = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.MyProducts.Add(newProduct);
            productsViewModel.RefreshList();

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
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }
        private async void LoadCategories(Category category)
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection(false);
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadCategoriesFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.Category = this.MyCategories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            this.IsRunning = false;
            this.IsEnabled = true;

        }
        private async Task<bool> LoadCategoriesFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.apiService.GetList<Category>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyCategories = (List<Category>)response.Result;
            return true;
        }
        private void RefreshList()
        {
            this.Categories = new ObservableCollection<Category>(this.MyCategories.OrderBy(c => c.Description));
        }
        #endregion

    }
}
