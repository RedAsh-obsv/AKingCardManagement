using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AKingCard
{
    public class CardItemText : DataCardItem
    {
        private Text textView;
        public CardItemText(long index, string name, Vector2 size, Vector2 position)
        {
            this.index = index;
            this.name = name;
            this.size = size;
            this.position = position;
        }
    }
}