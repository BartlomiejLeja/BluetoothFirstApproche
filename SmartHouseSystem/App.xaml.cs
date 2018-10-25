using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;
using SmartHouseSystem.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmartHouseSystem
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
        readonly IUnityContainer _container = new UnityContainer();
        readonly ISignalRService _signalRService =new SignalRService();
        private readonly ILightService _lightService = new LightService();
        private readonly IChartService _chartService = new ChartService();
        private readonly ISpeechRecognizerService _speechRecognizerService = new SpeechRecognizerService();
           
        private ExtendedSplashScreen _extendedSplashScreen;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
          
            _signalRService.ConnectionBuilder(_chartService, _lightService);
            //TODO This is propobly to remove after implementing onconnection method in azure server app
           // _signalRService.CurrentConnectionStatePropertyChanged += _signalRService_PropertyChanged;
            _signalRService.LightsListLoadedPropertyChanged += _signalRService_LightsLoadedChange;
            ExtendedSplashScreenFactory = (splashscreen) => _extendedSplashScreen=new ExtendedSplashScreen(splashscreen);
            this.Suspending += OnSuspending;
        }

        private async Task InitSpeechRecognizerAsync()
        {
            await _speechRecognizerService.SpeechRecognizerStartAsync(_signalRService);
            if (_speechRecognizerService.speechRecognizer.State == SpeechRecognizerState.Idle)
            {
                await _speechRecognizerService.speechRecognizer.ContinuousRecognitionSession.StartAsync();
            }
            else
                Debug.WriteLine("Speech recognizer not ready");
        }

        //TODO This is propobly to remove after implementing onconnection method in azure server app
//        private void _signalRService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//           //   _signalRService.InvokeCheckStatusOfLights(true);
//        }

        private void _signalRService_LightsLoadedChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () => { var t = NavigationService.Navigate("Main", null); });
           _chartService.ChartHandler( _lightService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            await InitSpeechRecognizerAsync();
        }
              
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // Register MvvmAppBase services with the container so that view models can take dependencies on them
            _container.RegisterInstance(_signalRService);
            _container.RegisterInstance<ILightService>(_lightService);
            _container.RegisterInstance<IChartService>(_chartService);
            _container.RegisterInstance<ISpeechRecognizerService>(_speechRecognizerService);
            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<IEventAggregator>(EventAggregator);
            // Register any app specific types with the container
            // Set a factory for the ViewModelLocator to use the container to construct view models so their 
            // dependencies get injected by the container
            ViewModelLocationProvider.SetDefaultViewModelFactory((viewModelType) => _container.Resolve(viewModelType));
     
          return base.OnInitializeAsync(args);
        }
        
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
