using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DataServices.Models;
using DataServices;
using Prism.Windows.Navigation;
using Prism.Commands;
using System.Windows.Input;
using ThinkWiseMan.Helpers;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using System.Text;

namespace ThinkWiseMan.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IXmlDataService XmlDataService;
        INavigationService NavigationService;
        IBackgroundTaskManager BackgroundTaskManager;

        public ICommand ChangeFavorites => new DelegateCommand(async () =>
        {
            CurrentWiseIdea.IsFavorite = !CurrentWiseIdea.IsFavorite;
            await XmlDataService.AddDeleteFavorite(_currentWiseIdea.Id, _currentWiseIdea.IsFavorite);

        });
        public ICommand GoToFavouritesCommand => new DelegateCommand(() => NavigationService.Navigate("Favourites", null));

        public ICommand GoToSettingsCommand => new DelegateCommand(() => NavigationService.Navigate("Settings", null));
        public ICommand GoToSelectedWiseIdea => new DelegateCommand<WiseIdeaModel>((current) => { CurrentWiseIdea = current; });
        public ICommand CopySelectedWiseIdea => new DelegateCommand(() =>
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(CurrentWiseIdea.Content);
            sb.AppendLine(CurrentWiseIdea.Author);
            dataPackage.SetText(sb.ToString());
            Clipboard.SetContent(dataPackage);

        });

        public ICommand ShareCommand => new DelegateCommand(() =>
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();

        });

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(CurrentWiseIdea.Content);
            sb.AppendLine(CurrentWiseIdea.Author);
            request.Data.SetText(sb.ToString());
            request.Data.Properties.Title = "Поделиться мудростью";
        }

        public MainPageViewModel(INavigationService navigationService, IBackgroundTaskManager backgroundTaskManager,
            IXmlDataService xmlDataService)
        {
            NavigationService = navigationService;
            BackgroundTaskManager = backgroundTaskManager;
            XmlDataService = xmlDataService;
        }

        private WiseIdeaModel _currentWiseIdea;

        public WiseIdeaModel CurrentWiseIdea
        {
            get { return _currentWiseIdea; }
            set
            {
                foreach (var item in Ideas.Where(x => x.Selected))
                {
                    item.Selected = false;
                }
                if (value != null)
                {
                    value.Selected = true;
                    _currentWiseIdea = value;

                    OnPropertyChanged("CurrentWiseIdea");

                }

            }
        }


        private ObservableCollection<WiseIdeaModel> _ideas = new ObservableCollection<WiseIdeaModel>();
        public ObservableCollection<WiseIdeaModel> Ideas
        {
            get
            {
                return _ideas;
            }
            set
            {
                _ideas = value;
                OnPropertyChanged("Ideas");
            }
        }


        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            
            var list = await GetIdeasToday();
            foreach (var item in list)
            {
                _ideas.Add(item);
            }
            _currentWiseIdea = _ideas[0];
            await BackgroundTaskManager.RegisterNotificationTaskAsync();
            base.OnNavigatedTo(e, viewModelState);

        }

        public async Task<IEnumerable<WiseIdeaModel>> GetIdeasToday()
        {
            return await XmlDataService.GetThoughtsByDayAsync(DateTime.Now.Day, DateTime.Now.Month);
        }
        public async Task<IEnumerable<WiseIdeaModel>> GetIdeasByDate(int day, int month)
        {
            return await XmlDataService.GetThoughtsByDayAsync(day, month);
        }


    }


}
