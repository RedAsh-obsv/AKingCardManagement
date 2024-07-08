using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public class CardTemplate
    {
        public long index { private set; get; }
        public string name { private set; get; }
        public Vector2 size { private set; get; }
        public List<CardItem> cardItems { private set; get; }
        public CardTemplate(string name, Vector2 size)
        {
            this.index = (DateTime.Now.Ticks * 1000);
            this.name = name;
            this.size = size;
            cardItems = new List<CardItem>();
            cardItems.Add(new CardItemImage(0, "Background", size, Vector2.zero));
        }

        public bool Equals(CardTemplate otherTemplate)
        {
            if (otherTemplate.index != index) return false;
            if (otherTemplate.name != name) return false;
            if (otherTemplate.size != size) return false;
            if (otherTemplate.index != index) return false;
            if (otherTemplate.cardItems.Count != cardItems.Count) return false;
            else
            {
                for (int i = 0; i < otherTemplate.cardItems.Count; i++)
                {
                    if(!otherTemplate.cardItems[i].Equals(cardItems[i].index)) return false;
                }
            }
            return true;
        }
    }
}