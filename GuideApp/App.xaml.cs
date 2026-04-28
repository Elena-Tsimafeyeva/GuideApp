using Microsoft.Extensions.DependencyInjection;
using GuideApp;

namespace GuideApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
    }
}