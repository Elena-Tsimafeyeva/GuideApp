using GuideApp.DB;
using GuideApp.Model;
using System.Collections.ObjectModel;


namespace GuideApp.ViewModel
{
    public class SearchPageVM : ViewModelBase
    {
        private DatabaseService _database = new DatabaseService();
        //Коллекция для UI со статьями, которая автоматически обновляет UI при изменениях
        public ObservableCollection<ArticleModel> DisplayedArticles { get; set; } = new();
        //_currentSection хранит фильтрованный список, чтобы можно было вернуть назад
        private List<ArticleModel> _currentSection;
        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                SearchArticles();
            }
        }
        public Command<int> LoadSectionCommand { get; }
        public Command<ArticleModel> OpenArticleCommand { get; }
        public Command SearchCommand { get; }

        public SearchPageVM()
        {
            LoadSectionCommand = new Command<int>(LoadSection);
            OpenArticleCommand = new Command<ArticleModel>(async article => await OpenArticle(article));

            SearchCommand = new Command(SearchArticles);
        }
        //Загрузка раздела
        private void LoadSection(int section)
        {
            List<ArticleModel> items;

            if (section == 0)
                items = _database.GetAllArticles();
            else
                items = _database.GetArticlesBySection(section);

            _currentSection = items;

            DisplayedArticles.Clear();

            foreach (var item in items)
                DisplayedArticles.Add(item);
        }
        //Открытие статьи
        private async Task OpenArticle(ArticleModel article)
        {
            if (article == null)
                return;

            try
            {
                var articleVM = new ArticlePageVM();
                articleVM.LoadArticle(article.IdArticle, article.Title);

                await Application.Current.MainPage.Navigation.PushAsync(
                    new GuideApp.ArticlePage
                    {
                        BindingContext = articleVM
                    });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }
        //Поиск возвращает список по текущему фильтру
        private void SearchArticles()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Перезагружаем текущий фильтр
                ReloadCurrent();
                return;
            }

            var results = _database.SearchArticles(SearchText);

            DisplayedArticles.Clear();

            foreach (var item in results.OrderBy(x => x.Title))
                DisplayedArticles.Add(item);
        }
        //Сброс списка. Если фильтра нет, грузит всё. Если есть секция, возвращает её
        private void ReloadCurrent()
        {
            List<ArticleModel> items;

            if (_currentSection == null)
                items = _database.GetAllArticles();
            else
                items = _currentSection;

            DisplayedArticles.Clear();

            foreach (var item in items.OrderBy(x => x.Title))
                DisplayedArticles.Add(item);
        }
    }
}
