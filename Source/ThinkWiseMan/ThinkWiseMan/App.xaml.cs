using Prism.Unity.Windows;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace ThinkWiseMan
{
    sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }
    }
}
