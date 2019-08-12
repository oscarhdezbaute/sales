namespace sales.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using sales.Helpers;
    using sales.Views;    
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion
        #region Commands
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(GoTo);
            }
        }
        #endregion
        #region Methods
        private void GoTo()
        {
            if (this.PageName == "LoginPage")
            {
                //Limpio los setting
                Settings.AccessToken = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion
    }
}
