using System;
using System.Collections.Generic;
using System.Text;

namespace GuideApp.Model
{
    public class ImagesModel
    {
        public int IdImages { get; set; }
        public int IdArticle { get; set; }
        public string ImagePath { get; set; }
        public int ImageOrder { get; set; }
        public ImagesModel(int idImages, int idArticle, string imagePath, int imageOrder)
        {
            IdImages = idImages;
            IdArticle = idArticle;
            ImagePath = imagePath;
            ImageOrder = imageOrder;
        }
    }
}
