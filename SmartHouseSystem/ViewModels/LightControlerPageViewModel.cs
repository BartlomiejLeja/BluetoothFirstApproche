using Prism.Commands;
using Prism.Windows.Mvvm;
using SmartHouseSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Core;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.ViewModels
{
    public class LightControlerPageViewModel : ViewModelBase
    {
        private readonly ILightService _lightService;

        private static string _lightOn = "ms-appx:///Images/lightTurnOn.png";
        private static string _lightOff = "ms-appx:///Images/lightTurnOff.jpg";
        
        public ObservableCollection<DisplayLightModel> DisplayLightModels { get; set; }
        
        public LightControlerPageViewModel( ISignalRService signalRService, ILightService lightService)
        {
            _lightService = lightService;
            DisplayLightModels = new ObservableCollection<DisplayLightModel>();

            foreach (var bulb in lightService.LightModelList)
            {
                DisplayLightModels.Add(
                    new DisplayLightModel()
                    {
                        Name = bulb.Name,
                        LightStatus = UriChanger(bulb.LightStatus),
                        LightBulbNumber = bulb.ID.ToString(),
                        Command = new DelegateCommand<string>(async (args) =>
                        {
                            bulb.LightStatus =!bulb.LightStatus;
                            await signalRService.InvokeTurnOnLight(
                                bulb.LightStatus,int.Parse(args));
                        })
                    });
            }
            lightService.StatusOfLightPropertyChanged += LightService_PropertyChanged;
        }

        private async void LightService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    foreach (var statsuModel in _lightService.LightModelList)
                    {
                        DisplayLightModels.First(l=>l.LightBulbNumber==(statsuModel.ID).ToString()).LightStatus= UriChanger(statsuModel.LightStatus);
                    }
                });
        }
       
        private string UriChanger(bool lightState)
        {
             return  lightState ? _lightOn : _lightOff;
        }
    }
}

