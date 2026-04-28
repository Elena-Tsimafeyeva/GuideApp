using GuideApp.Model;
using GuideApp.DB;
using GuideApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GuideApp.ViewModel
{
    public class ArticlePageVM : ViewModelBase
    {
        private DatabaseService _database = new DatabaseService();

        public string Title { get; set; }
        public ObservableCollection<ArticleContent> Content { get; set; } = new ObservableCollection<ArticleContent>();

        public void LoadArticle(int articleId, string title)
        {
            Title = title;
            Content.Clear();
            var items = _database.GetArticleContent(articleId);
            foreach (var item in items)
                Content.Add(item);
        }
    }
}
