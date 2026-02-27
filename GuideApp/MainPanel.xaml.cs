using System.Security.AccessControl;

namespace GuideApp;

public partial class MainPanel : ContentView
{
	public MainPanel()
	{
		InitializeComponent();
	}
    private async void MapBtn(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage());
        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
    }
    private async void SearchBtn(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new SearchPage());
        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
    }
    private async void CameraBtn(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CameraPage());
        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
    }
    private async void InfoBtn(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InfoPage());
        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
    }
}