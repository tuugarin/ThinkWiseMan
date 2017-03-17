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
using Microsoft.QueryStringDotNET;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.System;

namespace ThinkWiseMan.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        ISqlDataService SqlDataService;
        INavigationService NavigationService;
        IBackgroundTaskManager BackgroundTaskManager;

        public ICommand ChangeFavorites => new DelegateCommand(async () =>
        {
            CurrentWiseIdea.IsFavorite = !CurrentWiseIdea.IsFavorite;
            await SqlDataService.UpdateFavoritePropery(_currentWiseIdea.Id, _currentWiseIdea.IsFavorite);

        });
        public ICommand GoToStoreAndReview => new DelegateCommand(async () =>
        {
            const string productId = "9nbwt5sdv3qj";
            var uri = new Uri("ms-windows-store://review/?ProductId=" + productId);
            await Launcher.LaunchUriAsync(uri);
        });
        public ICommand GoToFavouritesCommand => new DelegateCommand(() => NavigationService.Navigate("Favourites", null));
        public ICommand GoToHomeCommand => new DelegateCommand(() => { NavigationService.Navigate("Main", null); });
        public ICommand GoToSettingsCommand => new DelegateCommand(() => NavigationService.Navigate("Settings", null));
        public ICommand GoToSelectedWiseIdea => new DelegateCommand<WiseIdeaPresentModel>((current) => { CurrentWiseIdea = current; });
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
            request.Data.Properties.Title = "Мысли мудрых";
        }

        public MainPageViewModel(INavigationService navigationService, IBackgroundTaskManager backgroundTaskManager,
            ISqlDataService sqlDataService)
        {
            NavigationService = navigationService;
            BackgroundTaskManager = backgroundTaskManager;
            SqlDataService = sqlDataService;
        }

        private bool isVisibleHomeButton = false;

        public bool IsVisibleHomeButton
        {
            get { return isVisibleHomeButton; }
        }


        private WiseIdeaPresentModel _currentWiseIdea;

        public WiseIdeaPresentModel CurrentWiseIdea
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


        private ObservableCollection<WiseIdeaPresentModel> _ideas = new ObservableCollection<WiseIdeaPresentModel>();
        public ObservableCollection<WiseIdeaPresentModel> Ideas
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

            IEnumerable<WiseIdeaPresentModel> list;
            if (e.Parameter != null)
            {
                QueryString qargs = QueryString.Parse(e.Parameter as string);
                int day = int.Parse(qargs["day"]);
                int month = int.Parse(qargs["month"]);
                list = await GetIdeasByDate(day, month);
                if (!(day == DateTime.Now.Day && month == DateTime.Now.Month))
                    isVisibleHomeButton = true;
            }
            else
            {
                list = await GetIdeasToday();
                isVisibleHomeButton = false;
            }
            foreach (var item in list)
            {
                _ideas.Add(item);
            }
            CurrentWiseIdea = Ideas[0];
            await BackgroundTaskManager.RegisterNotificationTaskAsync();
            base.OnNavigatedTo(e, viewModelState);

        }

        public async Task<IEnumerable<WiseIdeaPresentModel>> GetIdeasToday()
        {
            return await SqlDataService.GetThoughtsByDayAsync(DateTime.Now.Day, DateTime.Now.Month);
        }
        public async Task<IEnumerable<WiseIdeaPresentModel>> GetIdeasByDate(int day, int month)
        {
            return await SqlDataService.GetThoughtsByDayAsync(day, month);
        }


    }


}
