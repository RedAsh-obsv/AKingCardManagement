using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AKingCard
{
    public class CardItemImage : CardItem
    {
        private RawImage imageView;
        public CardItemImage(DataTemplate template, long index, string name, Vector2 size, Vector2 position)
        {
            this.template = template;
            this.index = index;
            this.name = name;
            this.size = size;
            this.position = position;
        }
    }
}
