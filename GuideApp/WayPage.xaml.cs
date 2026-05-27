using GuideApp.ViewModel;
namespace GuideApp;

public partial class WayPage : ContentPage
{
	public WayPage()
	{
		InitializeComponent();
		BindingContext = new WayPageVM();
    }
}