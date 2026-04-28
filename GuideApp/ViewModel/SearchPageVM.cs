using GuideApp.DB;
using GuideApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace GuideApp.ViewModel
{
    public class SearchPageVM : ViewModelBase
    {
        private DatabaseService _database = new DatabaseService();

        public ObservableCollection<ArticleModel> Articles { get; set; } = new();
        public Command<int> LoadSectionCommand { get; }
        public Command<ArticleModel> OpenArticleCommand { get; }
      
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

        public ObservableCollection<ArticleModel> SearchResults { get; set; } = new();

        public Command SearchCommand { get; }

        public SearchPageVM()
        {
            LoadSectionCommand = new Command<int>(LoadSection);
            OpenArticleCommand = new Command<ArticleModel>(async article => await OpenArticle(article));

            SearchCommand = new Command(SearchArticles);
        }

        private void LoadSection(int section)
        {
            Articles.Clear();
            var items = _database.GetArticlesBySection(section);
            foreach (var item in items)
                Articles.Add(item);
        }

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
        
        private void SearchArticles()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults.Clear();
                return;
            }

            var results = _database.SearchArticles(SearchText);

            SearchResults.Clear();
            foreach (var item in results)
                SearchResults.Add(item);
        }
    }
}
