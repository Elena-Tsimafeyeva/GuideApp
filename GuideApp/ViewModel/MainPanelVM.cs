using System.Windows.Input;
namespace GuideApp.ViewModel
{
    public class MainPanelVM : ViewModelBase
    {
        public ICommand OpenMapCommand { get; }
        public ICommand SearchInfoCommand { get; }
        public ICommand CameraQRCommand { get; }
        public ICommand AdditionalInfoCommand { get; }
        public ICommand ChooseWayCommand { get; }
        public MainPanelVM()
        {
            OpenMapCommand = new Command(async () => await OpenMap());
            SearchInfoCommand = new Command(async () => await SearchInfo());
            CameraQRCommand = new Command(async () => await CameraQR());
            AdditionalInfoCommand = new Command(async () => await AdditionalInfo());
            ChooseWayCommand = new Command(async () => await ChooseWayInfo());
        }
        private async Task OpenMap()
        {
            var newPage = new MapPage(); 
            await Application.Current.MainPage.Navigation.PushAsync(newPage);
            
            Application.Current.MainPage.Navigation.RemovePage(
                Application.Current.MainPage.Navigation.NavigationStack[0]);
        }
        private async Task SearchInfo()
        {
            var newPage = new SearchPage();
            await Application.Current.MainPage.Navigation.PushAsync(newPage);

            Application.Current.MainPage.Navigation.RemovePage(
                Application.Current.MainPage.Navigation.NavigationStack[0]);
        }
        private async Task CameraQR()
        {
            var newPage = new CameraPage();
            await Application.Current.MainPage.Navigation.PushAsync(newPage);

            Application.Current.MainPage.Navigation.RemovePage(
                Application.Current.MainPage.Navigation.NavigationStack[0]);
        }
        private async Task AdditionalInfo()
        {
            var newPage = new InfoPage();
            await Application.Current.MainPage.Navigation.PushAsync(newPage);

            Application.Current.MainPage.Navigation.RemovePage(
                Application.Current.MainPage.Navigation.NavigationStack[0]);
        }
        private async Task ChooseWayInfo()
        {
            var newPage = new WayPage();
            await Application.Current.MainPage.Navigation.PushAsync(newPage);

            Application.Current.MainPage.Navigation.RemovePage(
                Application.Current.MainPage.Navigation.NavigationStack[0]);
        }
    }
}
