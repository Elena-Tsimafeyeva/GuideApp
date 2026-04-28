using System.Security.AccessControl;
using GuideApp.ViewModel;
namespace GuideApp;

public partial class MainPanel : ContentView
{
	public MainPanel()
	{
		InitializeComponent();
		BindingContext = new MainPanelVM();
    }
}