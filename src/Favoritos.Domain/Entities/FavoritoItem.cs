using System;

namespace Favoritos.Domain.Entities
{
    public class FavoritoItem : Entity
    {
        public Guid IdOrigem { get; private set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public Uri Image { get; set; }
        public Decimal ReviewScore { get; set; }
        public Decimal Price { get; set; }


        public FavoritoItem(Guid idOrigem, string title, string brand, Uri image, Decimal reviewScore, Decimal price)
        {
            IdOrigem = idOrigem;
            Title = title;
            Brand = brand;
            Image = image;
            ReviewScore = reviewScore;
            Price = price;
        }
    }
}


