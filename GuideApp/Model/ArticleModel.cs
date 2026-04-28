using System;
using System.Collections.Generic;
using System.Text;

namespace GuideApp.Model
{
    public class ArticleModel
    {
        public int IdArticle { get; set; }
        public string Title { get; set; }
        public int SelectionNum { get; set; }
        public int QRCode { get; set; }

        public ArticleModel(int idArticle, string title,int selectionNum, int qrCode)
        {
            IdArticle = idArticle; Title = title; SelectionNum = selectionNum; QRCode = qrCode;
        }
    }
}
