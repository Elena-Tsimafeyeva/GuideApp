using GuideApp.ViewModel;
namespace GuideApp;

public partial class ArticlePage : ContentPage
{
	public ArticlePage()
	{
		InitializeComponent();
        BindingContext = new ArticlePageVM();
    }
}