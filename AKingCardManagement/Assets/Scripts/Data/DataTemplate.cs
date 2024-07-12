using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AKingCard
{
    public class DataTemplate
    {
        public long index { private set; get; }
        public string name { private set; get; }
        public Vector2 size { private set; get; }
        public List<DataCardItem> cardItems { private set; get; }
        public DataTemplate(string name, Vector2 size)
        {
            this.index = DateTime.Now.Ticks;
            this.name = name;
            this.size = size;
            cardItems = new List<DataCardItem>();
            cardItems.Add(new CardItemImage(this, 0, "Background", size, Vector2.zero));
        }

        public bool Equals(DataTemplate otherTemplate)
        {
            if (otherTemplate == null) return false;
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