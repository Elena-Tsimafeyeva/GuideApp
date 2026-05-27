

namespace GuideApp.Model
{
    public class ArticleModel
    {
        public int IdArticle { get; set; }
        public string Title { get; set; }
        public int SelectionNum { get; set; }
        public int QRCode { get; set; }
        public int Way {  get; set; }
        public int Point { get; set; }

        public ArticleModel(int idArticle, string title,int selectionNum, int qrCode, int way, int point)
        {
            IdArticle = idArticle; Title = title; SelectionNum = selectionNum; QRCode = qrCode; Way = way; Point = point;
        }
    }
}
