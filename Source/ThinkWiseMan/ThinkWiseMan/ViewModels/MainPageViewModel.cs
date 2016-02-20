using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using DataServices.Models;
using DataServices;
using Prism.Windows.Navigation;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Prism.Commands;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using ThinkWiseMan.Views;
using System.Windows.Input;
using ThinkWiseMan.Helpers;

namespace ThinkWiseMan.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IXmlDataService xmlDataService = new XmlDataService();
        INavigationService NavigationService;
        IBackgroundTaskManager BackgroundTaskManager;
        public ICommand GoToSettingsCommand => new DelegateCommand(() => NavigationService.Navigate("Settings", null));
        public MainPageViewModel(INavigationService navigationService, IBackgroundTaskManager backgroundTaskManager)
        {
            NavigationService = navigationService;
            BackgroundTaskManager = backgroundTaskManager;
        }
        private DateTimeOffset _currentDate;

        public DateTimeOffset CurrentDate
        {
            get
            {
                if (_currentDate == DateTimeOffset.MinValue) _currentDate = DateTime.Now;
                return _currentDate;
            }
            set
            {
                _currentDate = value;
                DateChanged();
            }
        }


        private ObservableCollection<WiseIdeaModel> _ideas;

        public ObservableCollection<WiseIdeaModel> Ideas
        {
            get
            {
                if (_ideas != null)
                    return _ideas;
                else
                {
                    _ideas = new ObservableCollection<WiseIdeaModel>();
                    return _ideas;
                }
            }
            set
            {
                _ideas = value;
            }
        }


        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var list = await GetIdeasToday();
            foreach (var item in list)
            {
                Ideas.Add(item);
            }
            await BackgroundTaskManager.RegisterNotificationTask();
            //   base.OnNavigatedTo(e, viewModelState);

        }

        public async void DateChanged()
        {
            Ideas.Clear();
            var list = await GetIdeasByDate(_currentDate.Day, _currentDate.Month);
            foreach (var item in list)
            {
                Ideas.Add(item);
            }
        }

        public async Task<IEnumerable<WiseIdeaModel>> GetIdeasToday()
        {
            return await xmlDataService.GetThoughtsByDayAsync(DateTime.Now.Day, DateTime.Now.Month);
        }
        public async Task<IEnumerable<WiseIdeaModel>> GetIdeasByDate(int day, int month)
        {
            return await xmlDataService.GetThoughtsByDayAsync(day, month);
        }


    }


}
