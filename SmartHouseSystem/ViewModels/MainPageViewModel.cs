using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using SmartHouseSystem.Model;
using SmartHouseSystem.Services;
using System;
using System.Collections.Generic;
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
        Random rand = new Random();
        public List<StatusModel> statusList;

        public bool IsPaneOpen
        {
            get { return isPaneOpen; }

            set { SetProperty(ref isPaneOpen, value); }
        }

        public List<StatusModel> StatusList
        {
            get { return statusList; }

            set { SetProperty(ref statusList, value); }
        }


        public MainPageViewModel(INavigationService navigationService, ISignalRService signalRService)
        {
            StatusList = new List<StatusModel>
            {
                  new StatusModel("bullb1",rand.Next(0, 200)),
                  new StatusModel("bullb2",rand.Next(0, 200)),
                  new StatusModel("bullb3",rand.Next(0, 200)),
            };
            Debug.WriteLine("TestMainViewModel");
            this.navigationService = navigationService;
            //OpenHamburgerMenuCommand = new DelegateCommand(async () =>
            //{
            //    IsPaneOpen = !isPaneOpen;
            //});
            OpenHamburgerMenuCommand = new DelegateCommand(HamburgerMenuButton);
            CameraViewerPageCommand = new DelegateCommand(CameraViewerPage);
            ESPViewerPageCommand = new DelegateCommand(LightControlerPage);
        //    signalRService.Connect();
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
