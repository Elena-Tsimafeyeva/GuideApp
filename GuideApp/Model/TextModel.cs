using System;
using System.Collections.Generic;
using System.Text;

namespace GuideApp.Model
{
    public class TextModel
    {
        public int IdText { get; set; }
        public int IdArticle { get; set; }
        public string Text { get; set; }
        public int TextOrder { get; set; }
        public TextModel(int idText, int idArticle, string text, int textOrder)
        {
            IdText = idText;
            IdArticle = idArticle;
            Text = text;
            TextOrder = textOrder;
        }
    }
}
