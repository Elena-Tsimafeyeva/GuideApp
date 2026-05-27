using GuideApp.DB;
using GuideApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GuideApp.ViewModel
{
    public class WayPageVM : ViewModelBase
    {
        private DatabaseService _database = new DatabaseService();

        public ObservableCollection<ArticleModel> DisplayedArticles { get; set; } = new();

        public List<string> Ways { get; } = new()
    {
        "Путь 1",
        "Путь 2"
    };

        private string selectedWay;
        public string SelectedWay
        {
            get => selectedWay;
            set
            {
                selectedWay = value;
                OnPropertyChanged();
                LoadWay();
            }
        }

        public Command<ArticleModel> OpenArticleCommand { get; }

        public WayPageVM()
        {
            OpenArticleCommand = new Command<ArticleModel>(async a => await OpenArticle(a));

            // авто-загрузка первого пути
            //SelectedWay = Ways[0];
        }

        private void LoadWay()
        {
            if (string.IsNullOrEmpty(SelectedWay))
                return;

            int way = SelectedWay == "Путь 1" ? 1 : 2;

            var items = _database.GetArticlesByWay(way);

            DisplayedArticles.Clear();

            foreach (var item in items.OrderBy(x => x.Point))
                DisplayedArticles.Add(item);
        }

        private async Task OpenArticle(ArticleModel article)
        {
            if (article == null)
                return;

            var vm = new ArticlePageVM();
            vm.LoadArticle(article.IdArticle, article.Title);

            await Application.Current.MainPage.Navigation.PushAsync(
                new ArticlePage
                {
                    BindingContext = vm
                });
        }
    }
}