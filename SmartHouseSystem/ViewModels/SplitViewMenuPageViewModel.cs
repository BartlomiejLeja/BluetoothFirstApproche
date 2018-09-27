using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartHouseSystem.ViewModels
{
    public class SplitViewMenuPageViewModel : ViewModelBase
    {
        private const string CurrentPageTokenKey = "CurrentPageToken";
        private readonly Dictionary<PageTokens, bool> _canNavigateLookup;
        private PageTokens _currentPageToken;
        private readonly INavigationService _navigationService;
        private readonly ISessionStateService _sessionStateService;
        public ObservableCollection<MenuItemViewModel> MenuItemsList { get; set; }

        public SplitViewMenuPageViewModel(IEventAggregator eventAggregator, INavigationService navigationService, ISessionStateService sessionStateService)
        {
            eventAggregator.GetEvent<NavigationStateChangedEvent>().Subscribe(OnNavigationStateChanged);
            _navigationService = navigationService;
            _sessionStateService = sessionStateService;
          
            MenuItemsList = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { DisplayName = "Statistics", FontIcon = "\ue9d9", Command = new DelegateCommand(() => NavigateToPage(PageTokens.Main), () => CanNavigateToPage(PageTokens.Main)) },
                new MenuItemViewModel { DisplayName = "Light control", FontIcon = "\ue781", Command = new DelegateCommand(() => NavigateToPage(PageTokens.LightControler), () => CanNavigateToPage(PageTokens.LightControler)) }
            };

            _canNavigateLookup = new Dictionary<PageTokens, bool>();

            foreach (PageTokens pageToken in Enum.GetValues(typeof(PageTokens)))
            {
                _canNavigateLookup.Add(pageToken, true);
            }

            if (!_sessionStateService.SessionState.ContainsKey(CurrentPageTokenKey)) return;
            // Resuming, so update the menu to reflect the current page correctly
            if (!Enum.TryParse(_sessionStateService.SessionState[CurrentPageTokenKey].ToString(),
                out PageTokens currentPageToken)) return;
            UpdateCanNavigateLookup(currentPageToken);
            RaiseCanExecuteChanged();
        }

        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            if (!Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty),
                out PageTokens currentPageToken)) return;
            _sessionStateService.SessionState[CurrentPageTokenKey] = currentPageToken.ToString();
            UpdateCanNavigateLookup(currentPageToken);
            RaiseCanExecuteChanged();
        }

        private void NavigateToPage(PageTokens pageToken)
        {
            if (!CanNavigateToPage(pageToken)) return;
            if (!_navigationService.Navigate(pageToken.ToString(), null)) return;
            UpdateCanNavigateLookup(pageToken);
            RaiseCanExecuteChanged();
        }

        private bool CanNavigateToPage(PageTokens pageToken)
        {
            return _canNavigateLookup[pageToken];
        }

        private void UpdateCanNavigateLookup(PageTokens navigatedTo)
        {
            _canNavigateLookup[_currentPageToken] = true;
            _canNavigateLookup[navigatedTo] = false;
            _currentPageToken = navigatedTo;
        }

        private void RaiseCanExecuteChanged()
        {
            foreach (var item in MenuItemsList)
            {
                (item.Command as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
    }
}
