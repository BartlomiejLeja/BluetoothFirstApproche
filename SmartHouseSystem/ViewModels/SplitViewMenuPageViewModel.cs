using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseSystem.ViewModels
{
   public class SplitViewMenuPageViewModel : ViewModelBase
    {
        private const string CurrentPageTokenKey = "CurrentPageToken";
        private Dictionary<PageTokens, bool> _canNavigateLookup;
        private PageTokens _currentPageToken;
        private INavigationService _navigationService;
        private ISessionStateService _sessionStateService;
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

            if (_sessionStateService.SessionState.ContainsKey(CurrentPageTokenKey))
            {
                // Resuming, so update the menu to reflect the current page correctly
                PageTokens currentPageToken;
                if (Enum.TryParse(_sessionStateService.SessionState[CurrentPageTokenKey].ToString(), out currentPageToken))
                {
                    UpdateCanNavigateLookup(currentPageToken);
                    RaiseCanExecuteChanged();
                }
            }
        }

        private void OnNavigationStateChanged(NavigationStateChangedEventArgs args)
        {
            PageTokens currentPageToken;
            if (Enum.TryParse(args.Sender.Content.GetType().Name.Replace("Page", string.Empty), out currentPageToken))
            {
                _sessionStateService.SessionState[CurrentPageTokenKey] = currentPageToken.ToString();
                UpdateCanNavigateLookup(currentPageToken);
                RaiseCanExecuteChanged();
            }
        }

        private void NavigateToPage(PageTokens pageToken)
        {
            if (CanNavigateToPage(pageToken))
            {
                if (_navigationService.Navigate(pageToken.ToString(), null))
                {
                    UpdateCanNavigateLookup(pageToken);
                    RaiseCanExecuteChanged();
                }
            }
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
                (item.Command as DelegateCommand).RaiseCanExecuteChanged();
            }
        }
    }
}
