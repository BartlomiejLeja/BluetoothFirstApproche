using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Input;

namespace SmartHouseSystem.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool isPaneOpen =false;
        private readonly INavigationService navigationService;
        public ICommand OpenHamburgerMenuCommand { get; private set; }
        public ICommand CameraViewerPageCommand { get; private set; }
        public ICommand ESPViewerPageCommand { get; private set; }

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }

            set { SetProperty(ref isPaneOpen, value); }
        }

        public MainPageViewModel(INavigationService navigationService)
        {
            Debug.WriteLine("TestMainViewModel");
            this.navigationService = navigationService;
            //OpenHamburgerMenuCommand = new DelegateCommand(async () =>
            //{
            //    IsPaneOpen = !isPaneOpen;
            //});
            OpenHamburgerMenuCommand = new DelegateCommand(HamburgerMenuButton);
            CameraViewerPageCommand = new DelegateCommand(CameraViewerPage);
            ESPViewerPageCommand = new DelegateCommand(LightControlerPage);
        }

        private void HamburgerMenuButton()
        {
            IsPaneOpen = !isPaneOpen;
        }

        private void CameraViewerPage()
        {
            navigationService.Navigate("CameraViewer", null);
        }
        private void LightControlerPage()
        {
            navigationService.Navigate("LightControler", null);
        }
    }
}
