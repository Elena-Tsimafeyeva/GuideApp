namespace GuideApp;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}
    private async void Article(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ArticlePage());
        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
    }
}